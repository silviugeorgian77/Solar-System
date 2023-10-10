
public class CSVReader
{
    private static ThreadPool threadPool = new ThreadPool(1);

    public delegate void OnGetCSVDataCompleted(
        string[][] result,
        string csvString,
        char splitChar
    );

    public static string[][] GetCSVData(string csvString, char splitChar)
    {
        string[] lines = csvString.Split("\n"[0]);
        string[][] data = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            data[i] = lines[i].Trim().Split(splitChar);
        }

        return data;
    }

    public static void GetCSVData(
        string csvString,
        char splitChar,
        OnGetCSVDataCompleted listener)
    {
        threadPool.AddTask(
            new GetCSVDataTask(csvString, splitChar, listener)
        );
    }

    private class GetCSVDataTask : WorkerThread.Task
    {
        private string csvString;
        private char splitChar;
        private string[][] result;
        private OnGetCSVDataCompleted listener;

        public GetCSVDataTask(
            string csvString,
            char splitChar,
            OnGetCSVDataCompleted listener)
        {
            this.csvString = csvString;
            this.splitChar = splitChar;
            this.listener = listener;
        }

        public void Disable()
        {
        }

        public void Enable()
        {   
        }

        public void Execute()
        {
            result = GetCSVData(csvString, splitChar);
        }

        public void OnFinished()
        {
            listener?.Invoke(result, csvString, splitChar);
        }
    }
}