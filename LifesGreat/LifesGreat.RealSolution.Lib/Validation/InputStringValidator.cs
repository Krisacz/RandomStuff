using System;
using System.Text.RegularExpressions;
using LifesGreat.RealSolution.Lib.Log;

namespace LifesGreat.RealSolution.Lib.Validation
{
    public class InputStringValidator : IInputValidator<string>
    {
        private readonly ILogger _logger;

        public InputStringValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsValid(string inputStr)
        {
            try
            {
                //Accept only A-Z letters ("jobs") and dependency character ">"
                var validCharacters = new Regex("[A-Z>]+");
                if (!string.Equals(validCharacters.Match(inputStr).Value, inputStr))
                {
                    _logger.AddError("Incorrect input string. Valid character are: A-Z and \">\".", null);
                    return false;
                }

                //Fail if there is double (or more) dependency characters in a row (">")
                var multiDependencyCharacter = new Regex(">>");
                if (multiDependencyCharacter.IsMatch(inputStr))
                {
                    _logger.AddError("Incorrect input string. Dependency character used multiple times in a row.", null);
                    return false;
                }

                //First character is ">"
                if (inputStr.Length > 0 && inputStr[0] == '>')
                {
                    _logger.AddError("Incorrect input string. Missing job. Input string starts with \">\".", null);
                    return false;
                }

                //Last character is ">"
                if (inputStr.Length > 0 && inputStr[inputStr.Length - 1] == '>')
                {
                    _logger.AddError("Incorrect input string. Missing dependent job. Input string ends with \">\".",
                        null);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.AddError("InputStringValidator >> IsValid", ex);
            }

            return false;
        }
    }
}
