namespace SurveyBasket.ApI.Helper
{
    public static class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template , Dictionary<string , string> TempModel)
        {

            var templatPath = $"{Directory.GetCurrentDirectory()}/Templets/{template}.html";
            var streemReader = new StreamReader(templatPath);
            var body = streemReader.ReadToEnd();
            streemReader.Close();

            foreach (var item in TempModel)
            {
                body = body.Replace(item.Key, item.Value);
            }
            return body;
        }

    }
}
