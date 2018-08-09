

namespace Logikos.Restoration.IdUtility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Transactions;
    using Logikos.Restoration.IdUtility.Models;    
  
    /// <summary>
    /// Read and load information about assets from a file.  Store in a database or a collection of objects.
    /// </summary>
    /// <remarks>
    /// File format is uses comma separated values.  Examples:
    /// <code>
    /// 20111F0D000F0007,339980
    /// 20111F0D000F00A2,339979
    /// </code>
    /// </remarks>
    public class TrackerLoader
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // The code below can be move to a library for use elsewhere.
        // The other project will need a service reference to the Gateway 
        // entity classes.
        //

        const string ClassName = "TrackerLoader";   // Right, but I don't like reflection everywhere.
        const string ExitText = "Exit";             // Used for function exit tracing.
        
        /// <summary>
        /// Field position of mac address in line of file.
        /// </summary>
        const int MacAddressColumn = 0;
        
        /// <summary>
        /// Field position of serial number in line of file.
        /// </summary>
        const int SerialNumberColumn = 1;

        /// <summary>
        /// Delimiters that separate fields.
        /// </summary>
        static readonly string[] LineDelimiters = { "," }; 

        /// <summary>
        /// Label text.
        /// </summary>
        const string TagSerialNumberType = "Tag";

        /// <summary>
        /// Load tracker asset information from a text reader (probably attached to a file) into the database.
        /// </summary>
        /// <param name="source">Text reader attached to a file or some other stream.</param>
        /// <param name="gatewayEntities">Entities linked to database definitions.</param>
        /// <param name="batchDateTime">Timestamp to associate with the whole batch.</param>
        public static void LoadTrackers(TextReader source, Gateway.GatewayEntities gatewayEntities, DateTime batchDateTime)
        {
            const string tag = ClassName + ".LoadTrackers:";
            Trace.TraceInformation(tag + string.Format("batchDateTime={0}", batchDateTime));

            int tagNodeId = TagNodeId(gatewayEntities);
            string line;
            int linesProcessed = 0;

            using (var scope = new TransactionScope())  // Transaction may not be supported for ODATA sources.
            {
                try
                {
                    while ((line = source.ReadLine()) != null)
                    {
                        Trace.TraceInformation(tag + "Processing \"" + line + "\"");

                        linesProcessed++;

                        string[] splitLine = line.Split(LineDelimiters, StringSplitOptions.None);

                        if (splitLine.Length != 2)
                        {
                            string errorText = string.Format("Format error on line {0}.", linesProcessed);
                            Trace.TraceError(errorText);
                            throw new FormatException(errorText);
                        }

                        AddTracker(
                            gatewayEntities,
                            batchDateTime,
                            tagNodeId,
                            splitLine[MacAddressColumn],
                            splitLine[SerialNumberColumn]);
                    }
                    scope.Complete();
                }
                catch (FormatException)
                {
                    Trace.TraceError(tag + "Format error.");
                }
                catch (Exception)
                {
                    Trace.TraceError(tag + "Unknown error.");
                }
            }

            Trace.TraceInformation(tag + ExitText);
        }

        /// <summary>
        /// Find the integer type to be used for database relationships in the list of all node types.
        /// </summary>
        /// <param name="gatewayEntities"></param>
        /// <returns></returns>
        private static int TagNodeId(Gateway.GatewayEntities gatewayEntities)
        {
            var nodeTypes = gatewayEntities.SerialNumberTypes;
            int tagNodeId = -1;

            foreach (var nodeType in nodeTypes)
            {
                if (nodeType.Name == TagSerialNumberType)
                {
                    tagNodeId = nodeType.Id;
                    break;
                }
            }

            return tagNodeId;
        }

        /// <summary>
        /// Add a tracker and serial number to the database.
        /// </summary>
        /// <param name="gatewayEntities"></param>
        /// <param name="macAddress">Expected to represent a 32-bit integer in hex format.</param>
        /// <param name="serialNumber"></param>
        /// <exception cref="System.FormatException">One or both parameters could not be parsed.</exception>
        /// <exception cref="System.OverflowException">One or both parameters represented numbers that were too large.</exception>
        static void AddTracker(
            Gateway.GatewayEntities gatewayEntities,
            DateTime batchDate,
            int tagNodeId,
            string macAddress,
            string serialNumber)
        {
            UInt64 macDummy;

            macDummy = UInt64.Parse(macAddress, NumberStyles.AllowHexSpecifier);
            // The parsed value is not used but we want the exception thrown for invalid format and overflow.

            var tracker = new Gateway.Tracker();
            tracker.MobileId = macAddress;
            gatewayEntities.AddToTrackers(tracker);
            gatewayEntities.SaveChanges();

            var serial = new Gateway.SerialNumber();
            serial.BatchDate = batchDate;
            serial.TheSerialNumber = serialNumber;
            serial.SerialNumberTypeId = tagNodeId;
            serial.TrackerId = tracker.Id;

            gatewayEntities.AddToSerialNumbers(serial);

            gatewayEntities.SaveChanges();
        }
        
        /// <summary>
        /// Load trackers from a file into a lit of object.  Handy for on-screen display.
        /// </summary>
        /// <param name="filename">File to load data from.</param>
        /// <returns>List of job objects, if any are found.</returns>
        public static List<Job> LoadTrackersFromFileToList(string filename)
        {
            const string tag = ClassName + ".LoadTrackersFromFileToList:";

            Trace.TraceInformation(tag + string.Format("filename={0}", filename));

            var jobList = new List<Job>();

            if (string.IsNullOrEmpty(filename)) return jobList;

            string line;
            int linesProcessed = 0;

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(filename);
                DateTime batchDateTime = File.GetCreationTime(filename);

                while ((line = reader.ReadLine()) != null)
                {
                    Trace.TraceInformation(tag + "Processing \"" + line + "\"");

                    linesProcessed++;

                    string[] splitLine = line.Split(LineDelimiters, StringSplitOptions.None);

                    if (splitLine.Length != 2)
                    {
                        string errorText = string.Format("Format error on line {0}.", linesProcessed);
                        Trace.TraceError(errorText);
                        throw new FormatException(errorText);
                    }

                    var job = new Job();
                    job.BatchDate = batchDateTime;
                    job.AssetId = splitLine[MacAddressColumn];
                    job.SerialNumber = splitLine[SerialNumberColumn];

                    jobList.Add(job);
                }

                reader.Close();
            }

            catch (FileNotFoundException)
            {
                Trace.TraceError(tag + "File not found.");
            }
            catch (FormatException)
            {
                Trace.TraceError(tag + "Format error.");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            Trace.TraceInformation(tag + ExitText);

            return jobList;
        }
    }
}
