using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProjetDeSession.Models
{
    public partial class QuizExamenContext : DbContext
    {
        public QuizExamenContext()
        {
        }

        public QuizExamenContext(DbContextOptions<QuizExamenContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ItemOption> ItemOptions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionQuiz> QuestionQuizzes { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:ProjetDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerId).HasColumnName("answerID");

                entity.Property(e => e.OptionId).HasColumnName("optionID");

                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK__Answer__optionID__403A8C7D");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK__Answer__quizID__412EB0B6");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<ItemOption>(entity =>
            {
                entity.HasKey(e => e.OptionId)
                    .HasName("PK__ItemOpti__3D5DC3C12FDE9BAD");

                entity.ToTable("ItemOption");

                entity.Property(e => e.OptionId).HasColumnName("optionID");

                entity.Property(e => e.IsRight).HasColumnName("isRight");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.Text)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("text");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ItemOptions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__ItemOptio__quest__3B75D760");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Text)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("text");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Question__catego__38996AB5");
            });

            modelBuilder.Entity<QuestionQuiz>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.QuizId })
                    .HasName("PK__Question__AEC7805300155BD8");

                entity.ToTable("QuestionQuiz");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionQuizzes)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionQ__quest__440B1D61");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuestionQuizzes)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionQ__quizI__44FF419A");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("Quiz");

                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
