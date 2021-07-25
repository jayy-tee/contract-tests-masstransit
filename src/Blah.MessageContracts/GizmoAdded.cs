using System;

namespace Blah.MessageContracts
{
    public record GizmoAdded
    {
        public Guid Id { get; init;  }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
}