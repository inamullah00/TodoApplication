
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domin.Entities;

namespace ToDo.Infrastructure.Persistance
{
    public class ApplicationDbContext:IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoItemComment> TodoItemComments { get; set; }
        public DbSet<TodoItemShare> TodoItemShares { get; set; }
        public DbSet<TodoItemReminder> TodoItemReminders { get; set; }
        public DbSet<RecurringTodoItem> RecurringTodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            // ApplicationUser -> TodoItems (Cascade)
            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.TodoItems)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser -> TodoItemComments (Restrict to avoid cycles)
            modelBuilder.Entity<TodoItemComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // TodoItem -> TodoItemComments (Cascade)
            modelBuilder.Entity<TodoItemComment>()
                .HasOne(c => c.TodoItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser -> TodoItemShares (Restrict to avoid cycles)
            modelBuilder.Entity<TodoItemShare>()
                .HasOne(s => s.User)
                .WithMany(u => u.SharedTodoItems)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // TodoItem -> TodoItemShares (Cascade)
            modelBuilder.Entity<TodoItemShare>()
                .HasOne(s => s.TodoItem)
                .WithMany(t => t.SharedWith)
                .HasForeignKey(s => s.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // TodoItem -> TodoItemReminders (Cascade)
            modelBuilder.Entity<TodoItemReminder>()
                .HasOne(r => r.TodoItem)
                .WithMany(t => t.Reminders)
                .HasForeignKey(r => r.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // TodoItem -> RecurringTodoItems (Cascade)
            modelBuilder.Entity<RecurringTodoItem>()
                .HasOne(r => r.TodoItem)
                .WithMany(t => t.RecurringTasks)
                .HasForeignKey(r => r.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
