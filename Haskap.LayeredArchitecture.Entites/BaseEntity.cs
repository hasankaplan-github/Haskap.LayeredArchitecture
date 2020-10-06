using System;

namespace Haskap.LayeredArchitecture.Entites
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
