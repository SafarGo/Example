using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class TalantSignatureGenerator : MonoBehaviour
{
    void Start()
    {
        string talantId = "#237429"; // ���������� �������������
        int nonce = GetUnixTimestamp();

        string signature = GenerateMD5Signature(talantId, nonce);
        Debug.Log($"Signature: {signature}");
    }

    // ��������� �������� ������� � ������� Unix Timestamp
    int GetUnixTimestamp()
    {
        return (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    // ��������� MD5-����
    string GenerateMD5Signature(string talantId, int nonce)
    {
        string input = talantId + nonce.ToString();

        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2")); // �������������� � ����������������� ������
            }

            return sb.ToString();
        }
    }
}
