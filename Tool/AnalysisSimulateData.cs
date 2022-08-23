using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

/// <summary>
/// 模拟文件的处理类
/// </summary>
public class AnalysisSimulateData
{
    private static AnalysisSimulateData mInstance = null;

    public static AnalysisSimulateData instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new AnalysisSimulateData();
            }

            return mInstance;
        }
    }
    public AndroidJavaObject active = null; //显示的界面

    //bcg文件
    public string bcgFile = Application.persistentDataPath + "/entertech_vr_bcg.txt";

    //mceeg文件
    public string mceegFile = Application.persistentDataPath + "/entertech_vr_mceeg.txt";
    // bcg数据
    ArrayList bcgArray = new ArrayList();
    //mceeg数据
    ArrayList mceegArray = new ArrayList();

    /// <summary>
    /// 读取bcg模拟文件
    /// </summary>
    public void readBcgData()
    {
        bcgArray.Clear();
        var file = FileOperation.instance.readFile(bcgFile);
        var strArray = file.Split(',');
        foreach (var item in strArray)
        {
            try
            {
                if (item.Length > 0)
                {
                    bcgArray.Add(Convert.ToInt32(item.ToString()));
                }

            }
            catch (Exception ex)
            {
                AndroidJavaExtention.instance.toastText($"convert -{item}- {ex.Message} ", active);
            }

        }
    }

    /// <summary>
    /// 读取bcg模拟文件
    /// </summary>
    public void readMceegData()
    {
        mceegArray.Clear();
        var file = FileOperation.instance.readFile(mceegFile);
        var strArray = file.Split(',');
        foreach (var item in strArray)
        {
            try
            {
                if (item.Length > 0)
                {
                    mceegArray.Add(Convert.ToInt32(item.ToString()));
                }

            }
            catch (Exception ex)
            {
                AndroidJavaExtention.instance.toastText($"convert -{item}- {ex.Message} ", active);
            }

        }
    }

    /// <summary>
    /// bcg文件13个数据为一组，每次获取13个数据
    /// </summary>
    /// <param name="cursor">游标</param>
    /// <param name="dataArray">存储数据的数组</param>
    /// <returns>sbyte指针</returns>
    public async Task<IntPtr> getNextBcg(int cursor)
    {
        List<sbyte> bcgList = new List<sbyte>();
        var currentCursor = cursor * 13;
        for (int i = currentCursor; i < currentCursor + 13; i++)
        {
            int value = (int)bcgArray[i];
            var sByte = value < 128 ? value : value - 256;
            bcgList.Add(Convert.ToSByte(sByte));
        }
        var temp = bcgList.ToArray();
        // AndroidJavaExtention.instance.toastText($"{temp[0]}, {temp[1]}, {temp[2]}, {temp.Length}", active);
        IntPtr jArrPtr = AndroidJNIHelper.ConvertToJNIArray(temp);
        return jArrPtr;

    }

    /// <summary>
    /// mceeg文件124个数据为一组，每次获取124个数据
    /// </summary>
    /// <param name="cursor">游标</param>
    /// <param name="dataArray">存储数据的数组</param>
    /// <returns>sbyte指针</returns>
    public async Task<IntPtr> getNextMceeg(int cursor)
    {
        List<sbyte> mceegList = new List<sbyte>();
        var currentCursor = cursor * 124;
        for (int i = currentCursor; i < currentCursor + 124; i++)
        {
            int value = (int)mceegArray[i];
            var sByte = value < 128 ? value : value - 256;
            mceegList.Add(Convert.ToSByte(sByte));
        }
        var temp = mceegList.ToArray();
        IntPtr jArrPtr = AndroidJNIHelper.ConvertToJNIArray(temp);
        return jArrPtr;

    }
}
