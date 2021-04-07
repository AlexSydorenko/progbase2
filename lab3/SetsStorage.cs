using System;
using System.IO;
using System.Text;

class SetsStorage
{
    public ISetInt ReadSet(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }
        ISetInt set = new ArraySetInt();
        StreamReader sr = new StreamReader(filePath);
        string s = "";
        while (true)
        {
            s = sr.ReadLine();
            if (s == null)
            {
                break;
            }
            string[] values = s.Split(",");
            foreach (string item in values)
            {
                int parsedValue = 0;
                if (int.TryParse(item, out parsedValue))
                {
                    set.Add(parsedValue);
                }
                else
                {
                    return null;
                }
            }
        }
        sr.Close();
        return set;
    }

    public void WriteSet(string filePath, ISetInt set)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        StringBuilder sb = new StringBuilder();
        int[] setArray = new int[set.GetCount()];
        set.CopyTo(setArray);
        for (int i = 0; i < setArray.Length; i++)
        {
            if (i == setArray.Length - 1)
            {
                sb.Append(setArray[i]);
                break;
            }
            sb.Append(setArray[i] + ",");
        }
        StreamWriter sw = new StreamWriter(filePath);
        StringReader sr = new StringReader(sb.ToString());
        string s = "";
        while (true)
        {
            s = sr.ReadLine();
            if (s == null)
            {
                break;
            }
            sw.WriteLine(s);
        }
        sw.Close();
        sr.Close();
    }
}
