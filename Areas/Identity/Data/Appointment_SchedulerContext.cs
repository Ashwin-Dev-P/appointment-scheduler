﻿using Appointment_Scheduler.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Appointment_Scheduler.Models;

namespace Appointment_Scheduler.Data;

public class Appointment_SchedulerContext : IdentityDbContext<Appointment_SchedulerUser>
{
    public Appointment_SchedulerContext(DbContextOptions<Appointment_SchedulerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Appointment_Scheduler.Models.Schedule>? Schedule { get; set; }
}
