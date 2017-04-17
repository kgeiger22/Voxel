using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer {

    public static bool Check_Gen_Folder(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        } else
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }catch(System.Exception e)
            {
                Logger.MainLog.log(e.StackTrace.ToString());
            }
        }
        return false;
    }
	public static void Serialize_ToFile<T>(string path, string filename, string extention, T _DATA) where T : class
    {
        if (Check_Gen_Folder(path))
        {
            try
            {
                using (Stream s = File.OpenWrite(string.Format("{0}{1}.{2}", path, filename, extention)))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    f.Serialize(s, _DATA);
                }
            } catch(System.Exception e)
            {
                Logger.Log(e.StackTrace.ToString());
            }


        }
        else throw new System.Exception("Can't get correct directory");
    }

    public static void Serialize_ToFile_FullPath<T>(string path, T _DATA) where T : class
    {

        {
            try
            {
                using (Stream s = File.OpenWrite(path))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    f.Serialize(s, _DATA);
                }
            }
            catch (System.Exception e)
            {
                Logger.Log(e.StackTrace.ToString());
            }


        }
    }

    public static T Deserialize_From_File<T>(string path) where T : class
    {
        if (File.Exists(path))
        {
            try
            {
                using (Stream s = File.OpenRead(path))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    return f.Deserialize(s) as T;
                }
            }catch (System.Exception e)
            {
                Logger.Log(e.StackTrace.ToString() + "Error in deserialization");

            }
        }
        else throw new System.Exception("File cannot be found");
        return null;
    }
}
