namespace PockWeb.For.Test.Model
{
    public class ResponseModel
    {
        public ResponseModel(bool sucess, string message)
        {
            Sucess = sucess;
            Message = message;
        }

        public bool Sucess { get; private set; }
        public string Message { get; private set; }
    }

}
