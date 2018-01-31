
using System;
using System.Collections.Generic;
using SampleOidc.Models;
using Microsoft.EntityFrameworkCore;

namespace SampleOidc.Data
{
    public class ContactsDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
            .HasIndex(x => x.UserId);

            modelBuilder.Entity<Contact>()
            .Property(x => x.UserId)
            .IsRequired();

            modelBuilder.Entity<Address>()
            .HasOne(x => x.Contact)
            .WithMany(x => x.Addresses)
            .IsRequired();

            modelBuilder.Entity<Email>()
            .HasOne(x => x.Contact)
            .WithMany(x => x.Emails)
            .IsRequired();

            modelBuilder.Entity<Phone>()
            .HasOne(x => x.Contact)
            .WithMany(x => x.Phones)
            .IsRequired();
        }
    }
}