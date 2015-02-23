using AutoMapper;
using CoachSeek.Data.Model;
using System;

namespace CoachSeek.Domain.Entities
{
    public class Business2
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get; protected set; }


        public Business2(Guid id, 
            string name, 
            string domain)
        {
            Id = id;
            Name = name;
            Domain = domain;
        }

        public Business2()
        {
            Id = Guid.NewGuid();
        }

        // Minimal Unit testing constructor.
        public Business2(Guid id)
            : this()
        {
            Id = id;
        }


        public Business2Data ToData()
        {
            return Mapper.Map<Business2, Business2Data>(this);
        }
    }
}