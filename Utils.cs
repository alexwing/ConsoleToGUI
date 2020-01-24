using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Utils
{
    // Start is called before the first frame update
    public static float StringToFloat(string Value)
    {
        //  Value = Value.Replace(".", ",");

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        float valueFloat = float.Parse(Value, NumberStyles.Any, ci);

        //   float valueFloat = (float)System.Convert.ToDouble(Value);
        // float valueFloat = float.Parse(Value);
        return valueFloat;
    }

    public static string GetPath()
    {

        string screensPath;
            #if UNITY_ANDROID && !UNITY_EDITOR
			                                            screensPath = Application.temporaryCachePath;       // Also you can create a custom folder, like: screensPath = "/sdcard/DCIM/RegionCapture";

            #elif UNITY_IPHONE && !UNITY_EDITOR
                                                        screensPath = Application.temporaryCachePath;       // Also you can use persistent DataPath on IOS: screensPath = Application.persistentDataPath;
            #elif UNITY_IPHONE && !UNITY_EDITOR
                                                        screensPath = Application.temporaryCachePath;       // Also you can use persistent DataPath on IOS: screensPath = Application.persistentDataPath;
            #elif UNITY_WSA && !UNITY_EDITOR
                                                        screensPath = Application.persistentDataPath;       // Also you can use persistent DataPath on IOS: screensPath = Application.persistentDataPath;
            #else
                    screensPath = Application.persistentDataPath;    // Editor Mode

            #endif

        //  Debug.Log("screensPath: " + screensPath);
        return screensPath;
    }



    public static string JSONSerialize<T>(T obj)
    {
        string retVal = String.Empty;
        using (MemoryStream ms = new MemoryStream())
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            serializer.WriteObject(ms, obj);
            var byteArray = ms.ToArray();
            retVal = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }
        return retVal;
    }


}
