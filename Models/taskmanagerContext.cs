using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace taskmanagerBackendC.Models
{
    public partial class taskmanagerContext : DbContext
    {
        public taskmanagerContext()
        {
        }

        public taskmanagerContext(DbContextOptions<taskmanagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<GroupsTasks> GroupsTasks { get; set; }
        public virtual DbSet<GroupsUser> GroupsUser { get; set; }
        public virtual DbSet<Labels> Labels { get; set; }
        public virtual DbSet<LabelsTasks> LabelsTasks { get; set; }
        public virtual DbSet<Migrations> Migrations { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=taskmanager;Uid=root;Pwd=root;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.ToTable("groups", "taskmanager");

                entity.HasIndex(e => e.AdminId)
                    .HasName("groups_admin_id_foreign");

                entity.HasIndex(e => e.Name)
                    .HasName("groups_name_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.AdminId)
                    .HasColumnName("admin_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("groups_admin_id_foreign");
            });

            modelBuilder.Entity<GroupsTasks>(entity =>
            {
                entity.ToTable("groups_tasks", "taskmanager");

                entity.HasIndex(e => e.GroupsId)
                    .HasName("groups_tasks_groups_id_foreign");

                entity.HasIndex(e => e.TasksId)
                    .HasName("groups_tasks_tasks_id_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.GroupsId)
                    .HasColumnName("groups_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TasksId)
                    .HasColumnName("tasks_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Groups)
                    .WithMany(p => p.GroupsTasks)
                    .HasForeignKey(d => d.GroupsId)
                    .HasConstraintName("groups_tasks_groups_id_foreign");

                entity.HasOne(d => d.Tasks)
                    .WithMany(p => p.GroupsTasks)
                    .HasForeignKey(d => d.TasksId)
                    .HasConstraintName("groups_tasks_tasks_id_foreign");
            });

            modelBuilder.Entity<GroupsUser>(entity =>
            {
                entity.ToTable("groups_user", "taskmanager");

                entity.HasIndex(e => e.GroupsId)
                    .HasName("groups_user_groups_id_foreign");

                entity.HasIndex(e => e.UserId)
                    .HasName("groups_user_user_id_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.GroupsId)
                    .HasColumnName("groups_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Groups)
                    .WithMany(p => p.GroupsUser)
                    .HasForeignKey(d => d.GroupsId)
                    .HasConstraintName("groups_user_groups_id_foreign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupsUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("groups_user_user_id_foreign");
            });

            modelBuilder.Entity<Labels>(entity =>
            {
                entity.ToTable("labels", "taskmanager");

                entity.HasIndex(e => e.UserId)
                    .HasName("labels_user_id_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.LabelColor)
                    .IsRequired()
                    .HasColumnName("label_color")
                    .HasColumnType("char(10)")
                    .HasDefaultValueSql("white");

                entity.Property(e => e.LabelDescription)
                    .IsRequired()
                    .HasColumnName("label_description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabelTitle)
                    .IsRequired()
                    .HasColumnName("label_title")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("labels_user_id_foreign");
            });

            modelBuilder.Entity<LabelsTasks>(entity =>
            {
                entity.ToTable("labels_tasks", "taskmanager");

                entity.HasIndex(e => e.LabelsId)
                    .HasName("labels_tasks_labels_id_foreign");

                entity.HasIndex(e => e.TasksId)
                    .HasName("labels_tasks_tasks_id_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.LabelsId)
                    .HasColumnName("labels_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.TasksId)
                    .HasColumnName("tasks_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Labels)
                    .WithMany(p => p.LabelsTasks)
                    .HasForeignKey(d => d.LabelsId)
                    .HasConstraintName("labels_tasks_labels_id_foreign");

                entity.HasOne(d => d.Tasks)
                    .WithMany(p => p.LabelsTasks)
                    .HasForeignKey(d => d.TasksId)
                    .HasConstraintName("labels_tasks_tasks_id_foreign");
            });

            modelBuilder.Entity<Migrations>(entity =>
            {
                entity.ToTable("migrations", "taskmanager");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Batch)
                    .HasColumnName("batch")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Migration)
                    .IsRequired()
                    .HasColumnName("migration")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.ToTable("tasks", "taskmanager");

                entity.HasIndex(e => e.UserId)
                    .HasName("tasks_user_id_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("enum('done','working')")
                    .HasDefaultValueSql("working");

                entity.Property(e => e.TaskDescription)
                    .IsRequired()
                    .HasColumnName("task_description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TaskTitle)
                    .IsRequired()
                    .HasColumnName("task_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tasks_user_id_foreign");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users", "taskmanager");

                entity.HasIndex(e => e.Email)
                    .HasName("users_email_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });
        }
    }
}
