using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Exceptions
{
    
    public class ChargeGroupAlreadyExistException : AppException
    {
        public override string Code { get; } = "charge_group_already_exists";
        public Guid Id { get; }

        public ChargeGroupAlreadyExistException(Guid id) : base($"Charge group with id: {id} already exists.")
            => Id = id;
    }
}
