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

    // Start is called before the first frame update
    public static int StringToInt(string Value)
    {
        //  Value = Value.Replace(".", ",");

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        int valueInt = int.Parse(Value, NumberStyles.Any, ci);

        //   float valueFloat = (float)System.Convert.ToDouble(Value);
        // float valueFloat = float.Parse(Value);
        return valueInt;
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

    public static string GetAppPath()
    {
        string AppRoot = Path.GetDirectoryName(Application.dataPath);
        if (Application.isEditor)
        {
            AppRoot = Path.Combine(AppRoot, "Build\\");
        }
        return AppRoot;
    }


    public static string GetAndroidInternalFilesDir()
    {
        string[] potentialDirectories = new string[]
        {
        "/storage/emulated/0",
        "/sdcard",
        "/storage",        
        "/storage/emulated/0",
        "/mnt/sdcard",
        "/storage/sdcard0",
        "/storage/sdcard1"
        };

        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < potentialDirectories.Length; i++)
            {
                if (Directory.Exists(potentialDirectories[i]))
                {
                    return potentialDirectories[i];
                }
            }
        }
        return "";
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
    public static string ConvertColumnNumberToLetter(int colNumber)
    {
        try
        {
            int n = 0;
            int c = 0;
            string s = null;
            n = colNumber;
            do
            {
                c = ((n - 1) % 26);
                s = Convert.ToChar(c + 65) + s;
                n = (n - c) / 26;
            } while (n > 0);
            return s;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static bool[] ConvertByteArrayToBoolArray(byte[] bytes)
    {
        System.Collections.BitArray b = new System.Collections.BitArray(bytes);
        bool[] bitValues = new bool[b.Count];
        b.CopyTo(bitValues, 0);
        Array.Reverse(bitValues);
        return bitValues;
    }

    /// <summary>
    /// Packs a bit array into bytes, most significant bit first
    /// </summary>
    /// <param name="boolArr"></param>
    /// <returns></returns>
    public static byte[] ConvertBoolArrayToByteArray(bool[] boolArr)
    {
        byte[] byteArr = new byte[(boolArr.Length + 7) / 8];
        for (int i = 0; i < byteArr.Length; i++)
        {
            byteArr[i] = ReadByte(boolArr, 8 * i);
        }
        return byteArr;
    }


    /// <summary>
    /// Reads a code of length 8 in an array of bits, padding with zeros
    /// </summary>
    /// <param name="rawbits"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    private static byte ReadByte(bool[] rawbits, int startIndex)
    {
        int n = rawbits.Length - startIndex;
        if (n >= 8)
        {
            return (byte)ReadCode(rawbits, startIndex, 8);
        }
        return (byte)(ReadCode(rawbits, startIndex, n) << (8 - n));
    }

    /// <summary>
    /// Reads a code of given length and at given index in an array of bits
    /// </summary>
    /// <param name="rawbits">The rawbits.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    private static int ReadCode(bool[] rawbits, int startIndex, int length)
    {
        int res = 0;
        for (int i = startIndex; i < startIndex + length; i++)
        {
            res <<= 1;
            if (rawbits[i])
            {
                res++;
            }
        }
        return res;
    }

    public static Color ParseColor(string aCol)
    {
        var strings = aCol.Split(',');
        Color output = Color.black;
        for (var i = 0; i < strings.Length; i++)
        {
            output[i] = StringToFloat(strings[i])/256;
        }
        return output;
    }


}
