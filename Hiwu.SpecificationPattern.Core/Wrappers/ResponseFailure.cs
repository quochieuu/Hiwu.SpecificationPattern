﻿namespace Hiwu.SpecificationPattern.Core.Wrappers
{
    public class ResponseFailure
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<ErrorDetail> ErrorDetails { get; set; }
    }
}
