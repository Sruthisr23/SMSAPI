using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LPMessengerAPI.Model
{
    public partial class lpmessengerdbContext : DbContext
    {
        private readonly string _connectionString;
        public lpmessengerdbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public lpmessengerdbContext(DbContextOptions<lpmessengerdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiDocumentation> ApiDocumentations { get; set; }
        public virtual DbSet<ChatTemplate> ChatTemplates { get; set; }
        public virtual DbSet<GroupMaster> GroupMasters { get; set; }
        public virtual DbSet<MlMail> MlMails { get; set; }
        public virtual DbSet<MlSenderConfig> MlSenderConfigs { get; set; }
        public virtual DbSet<MlSendmail> MlSendmails { get; set; }
        public virtual DbSet<SenderPhoneConfig> SenderPhoneConfigs { get; set; }
        public virtual DbSet<ServicesAvailable> ServicesAvailables { get; set; }
        public virtual DbSet<SmsSend> SmsSends { get; set; }
        public virtual DbSet<SmsTransactionMessageStatus> SmsTransactionMessageStatuses { get; set; }
        public virtual DbSet<TemplateMaster> TemplateMasters { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WhatsappSend> WhatsappSends { get; set; }

        public virtual DbSet<WhatsappSenderConfig> WhatsappSenderConfigs { get; set; }
        public virtual DbSet<FileAttachment> FileAttachments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.24-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<ApiDocumentation>(entity =>
            {
                entity.ToTable("api_documentation");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceId).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Url).HasColumnType("mediumtext");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<ChatTemplate>(entity =>
            {
                entity.ToTable("Chat_Template");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MessageType).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.WhatsappSenderConfigId).HasColumnType("int(11)"); 
            });

            modelBuilder.Entity<GroupMaster>(entity =>
            {
                entity.ToTable("group_master");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ServiceId).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<MlMail>(entity =>
            {
                entity.ToTable("ml_mails");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupId).HasColumnType("int(11)");

                entity.Property(e => e.IsBodyHtml)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ReceiverEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.SenderEmail)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.SenderName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Otp)
                    .HasMaxLength(50);

                entity.Property(e => e.ReferenceId)
                    .HasMaxLength(50);

                entity.Property(e => e.TemplateId).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<MlSenderConfig>(entity =>
            {
                entity.ToTable("ml_sender_config");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<MlSendmail>(entity =>
            {
                entity.ToTable("ml_sendmails");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MailId).HasColumnType("int(11)");

                entity.Property(e => e.MailSendStatus).HasColumnType("bit(1)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<SenderPhoneConfig>(entity =>
            {
                entity.ToTable("sender_phone_config");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasColumnType("bigint(20)");

                entity.Property(e => e.SenderName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<ServicesAvailable>(entity =>
            {
                entity.ToTable("services_available");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<SmsSend>(entity =>
            {
                entity.ToTable("sms_send");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupId).HasColumnType("int(11)");

                entity.Property(e => e.Phone).HasColumnType("bigint(20)");

                entity.Property(e => e.TemplateId).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.Otp).HasMaxLength(50);

                entity.Property(e => e.RefferenceNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<SmsTransactionMessageStatus>(entity =>
            {
                entity.ToTable("sms_transaction_message_status");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.CreatedOn, "CreatedOn");

                entity.HasIndex(e => e.MessageDlrStatus, "MessageDlrStatus");

                entity.HasIndex(e => e.ModifiedOn, "ModifiedOn");

                entity.HasIndex(e => e.PhoneNumber, "PhoneNumber");

                entity.HasIndex(e => e.SendType, "SendType");

                entity.HasIndex(e => e.ServiceChannel, "ServiceChannel");

                entity.HasIndex(e => new { e.TransactionId, e.RequestId }, "TransactionId")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeliveredOn).HasColumnType("datetime");

                entity.Property(e => e.MessageDlrStatus).HasColumnType("int(11)");

                entity.Property(e => e.MessageText).HasMaxLength(2000);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.RequestId).HasMaxLength(80);

                entity.Property(e => e.SendType).HasColumnType("int(11)");

                entity.Property(e => e.ServiceChannel).HasColumnType("int(11)");

                entity.Property(e => e.TransactionId).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(10);

                entity.Property(e => e.Otp).HasMaxLength(10);

                entity.Property(e => e.RefferenceNumber).HasMaxLength(50);
            });
            modelBuilder.Entity<FileAttachment>(entity =>
            {
                entity.ToTable("File_Attachment");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ChatTemplateId).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FilePath).HasMaxLength(50);

                entity.Property(e => e.TemplateId).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<TemplateMaster>(entity =>
            {
                entity.ToTable("template_master");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ServiceId).HasColumnType("int(11)");

                entity.Property(e => e.Subject).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.IsBodyHtml)
                    .HasColumnType("bit(1)");

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasColumnType("bigint(20)");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Verification)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");
            });

            modelBuilder.Entity<WhatsappSend>(entity =>
            {
                entity.ToTable("whatsapp_send");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MessageType).HasMaxLength(50);

                entity.Property(e => e.Phone).HasColumnType("bigint(20)");

                entity.Property(e => e.SenderPhoneCofigId).HasColumnType("int(11)");

                entity.Property(e => e.TemplateId).HasMaxLength(50);

                entity.Property(e => e.ChatTemplateId).HasColumnType("int(11)");
                             

                entity.Property(e => e.IsSend)
                    .HasColumnType("bit(1)");

                entity.Property(e => e.GroupId).HasColumnType("int(11)");
                

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<WhatsappSenderConfig>(entity =>
            {
                entity.ToTable("whatsapp_sender_config");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasColumnType("bigint(20)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
