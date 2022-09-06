using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using UnityEngine;
using Moru;

public class JsonData : MonoBehaviour
{
    private static readonly string privateKey = "1718hy9dsf0jsdlfjds0pa9ids78ahgf81h32re";

    public static void Save<T>(object saveData, string path) where T : class
    {
        var sd = saveData;
        string jsonString = DataToJson(sd);
        string encryptString = Encrypt(jsonString);
        SaveFile(encryptString);
    }

    public static T Load<T>(string path) where T : class
    {
        //������ �����ϴ������� üũ.
        if (!File.Exists(GetPath()))
        {
            Debug.Log("���̺� ������ �������� ����.");
            return null;
        }

        string encryptData = LoadFile(GetPath());
        string decryptData = Decrypt(encryptData);

        Debug.Log(decryptData);

        T sd = JsonToData<T>(decryptData);
        return sd;
    }


    #region 

    //���̺� �����͸� json string���� ��ȯ
    static string DataToJson(object sd)
    {

        string jsonData = JsonUtility.ToJson(sd);
        Debug.LogWarning(jsonData);
        return jsonData;
    }

    //json string�� SaveData�� ��ȯ
    static T JsonToData<T>(string jsonData) where T : class
    {
        T sd = JsonUtility.FromJson<T>(jsonData);
        return sd;
    }

    ////////////////////////////////////////////////////////////////////////////////////////

    //json string�� ���Ϸ� ����
    static void SaveFile(string jsonData)
    {
        using (FileStream fs = new FileStream(GetPath(), FileMode.Create, FileAccess.Write))
        {
            //���Ϸ� ������ �� �ְ� ����Ʈȭ
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

            //bytes�� ���빰�� 0 ~ max ���̱��� fs�� ����
            fs.Write(bytes, 0, bytes.Length);
        }
    }

    //���� �ҷ�����
    static string LoadFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            //������ ����Ʈȭ ���� �� ���� ������ ����
            byte[] bytes = new byte[(int)fs.Length];

            //���Ͻ�Ʈ������ ���� ����Ʈ ����
            fs.Read(bytes, 0, (int)fs.Length);

            //������ ����Ʈ�� json string���� ���ڵ�
            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            return jsonString;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////

    private static string Encrypt(string data)
    {

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(results, 0, results.Length);

    }

    private static string Decrypt(string data)
    {

        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] resultArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(resultArray);
    }


    private static RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(privateKey);
        RijndaelManaged result = new RijndaelManaged();

        byte[] newKeysArray = new byte[16];
        System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;
        return result;
    }

    ////////////////////////////////////////////////////////////////////////////////////////

    //������ �ּҸ� ��ȯ
    static string GetPath()
    {
        //���ϰ�� �����
        Debug.Log(Application.persistentDataPath);
        return Path.Combine(Application.persistentDataPath, "PIN.EIW");
    }
    #endregion
}
