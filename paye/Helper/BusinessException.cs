using System;

namespace BaseSystemModel.Helper
{
    public class BusinessException : Exception
    {
        public object ErrorObject { set; get; }


        public BusinessException(string Message)
            : base(Message)
        {

        }
        public BusinessException(string Message, object Obj)
            : base(Message)
        {
            ErrorObject = Obj;
        }
    }
}
