﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Localization/CSV Loader")]
    public class CSVLoader : LocalizationLoader
    {
        [SerializeField]
        TextAsset localizationData;

        public override Dictionary<string, string> LoadLanguage(string locale)
        {
            //TODO just use standard csv parser
            
            string[] fileLines = localizationData.text.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
            string[] header = SplitCSVRow(fileLines[0]);
            int localeIndex = Array.IndexOf(header, locale);
            if(localeIndex >= 0)
            {
                Dictionary<string, string> loadedLines = new Dictionary<string, string>();
                for (int i = 1; i < fileLines.Length; ++i)
                {
                    string[] values = SplitCSVRow(fileLines[i]);
                    try {
                        string key = values[0];
                        string translation = values[localeIndex];
                        loadedLines.Add(key, translation);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Debug.LogWarning("Translation not found on line " + i.ToString());
                    }
                }
                return loadedLines;
            }
            else
            {
                // Locale not found
                return null;
            }
        }

        string[] SplitCSVRow(string row)
        {
            // TODO split by commas
            // However, if entry starts with ", ignore commas until matching quote
            // "" found withing " does not match, only single "
            // Remove " from either end and change "" to "
            throw new NotImplementedException();
        }
    }
}