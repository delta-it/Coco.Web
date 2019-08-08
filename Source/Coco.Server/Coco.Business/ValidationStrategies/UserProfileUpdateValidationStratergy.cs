﻿using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Business.ValidationStrategies
{
    public class UserProfileUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ErrorObject> Errors { get; set; }
        public UserProfileUpdateValidationStratergy() { }

        public bool IsValid<T>(T data)
        {
            var model = data as UserProfileUpdateModel;
            if (model == null)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model)));
            }

            if (model.Id <= 0)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.Id)));
            }

            if (string.IsNullOrWhiteSpace(model.Lastname))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.Lastname)));
            }

            if (string.IsNullOrWhiteSpace(model.Firstname))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.Firstname)));
            }

            if (string.IsNullOrWhiteSpace(model.DisplayName))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.DisplayName)));
            }

            return Errors == null || !Errors.Any();
        }

        public IEnumerable<ErrorObject> GetErrors(Exception e)
        {
            yield return new ErrorObject
            {
                Message = e.Message
            };
        }
    }
}
