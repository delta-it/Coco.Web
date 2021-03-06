﻿using Camino.Shared.Results.Errors;
using Camino.Core.Contracts.Strategies.Validations;
using System;
using System.Collections.Generic;

namespace Camino.Infrastructure.Strategies.Validations
{
    public class PhoneValidationStrategy : IValidationStrategy
    {
        private const string AdditionalPhoneNumberCharacters = "-.()";
        private const string ExtensionAbbreviationExtDot = "ext.";
        private const string ExtensionAbbreviationExt = "ext";
        private const string ExtensionAbbreviationX = "x";

        public IEnumerable<BaseErrorResult> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string valueAsString))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(valueAsString)));
                return false;
            }

            valueAsString = valueAsString.Replace("+", string.Empty).TrimEnd();
            valueAsString = RemoveExtension(valueAsString);

            bool digitFound = false;
            foreach (char c in valueAsString)
            {
                if (char.IsDigit(c))
                {
                    digitFound = true;
                    break;
                }
            }

            if (!digitFound)
            {
                Errors = GetErrors(new ArgumentException($"Non numberic of {nameof(digitFound)}"));
                return false;
            }

            foreach (char c in valueAsString)
            {
                if (!(char.IsDigit(c)
                    || char.IsWhiteSpace(c)
                    || AdditionalPhoneNumberCharacters.IndexOf(c) != -1))
                {
                    Errors = GetErrors(new ArgumentException($"Non numberic of {nameof(valueAsString)}"));
                    return false;
                }
            }

            return true;
        }

        private string RemoveExtension(string potentialPhoneNumber)
        {
            var lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(ExtensionAbbreviationExtDot, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + ExtensionAbbreviationExtDot.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(ExtensionAbbreviationExt, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + ExtensionAbbreviationExt.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(ExtensionAbbreviationX, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + ExtensionAbbreviationX.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            return potentialPhoneNumber;
        }

        private bool MatchesExtension(string potentialExtension)
        {
            potentialExtension = potentialExtension.TrimStart();
            if (potentialExtension.Length == 0)
            {
                return false;
            }

            foreach (char c in potentialExtension)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<BaseErrorResult> GetErrors(Exception exception)
        {
            yield return new BaseErrorResult()
            {
                Message = exception.Message
            };
        }
    }
}
