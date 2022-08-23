using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;

public class FileOperation
{
    private static FileOperation mInstance = null;

    public static FileOperation instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new FileOperation();
            }
            return mInstance;
        }
    }
    public string fileName = Application.persistentDataPath + "/EnterDemo.txt";

    public void saveFile(string[] text)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllLines(fileName, text);
            }
            else
            {
                File.Delete(fileName);
                File.WriteAllLines(fileName, text);
            }
        }
        catch (System.Exception ex)
        {

        }
    }

    public void saveFile(string text)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, text);
            }
            else
            {
                File.Delete(fileName);
                File.WriteAllText(fileName, text);
            }
        }
        catch (System.Exception ex)
        {

        }
    }

    public string readFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }

}