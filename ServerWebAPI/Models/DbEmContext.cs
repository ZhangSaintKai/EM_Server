using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerWebAPI.Models;

public partial class DbEmContext : DbContext
{
    public DbEmContext()
    {
    }

    public DbEmContext(DbContextOptions<DbEmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TContact> TContacts { get; set; }

    public virtual DbSet<TFile> TFiles { get; set; }

    public virtual DbSet<TGroupConversation> TGroupConversations { get; set; }

    public virtual DbSet<TGroupMember> TGroupMembers { get; set; }

    public virtual DbSet<TGroupMessage> TGroupMessages { get; set; }

    public virtual DbSet<TGroupMessageRead> TGroupMessageReads { get; set; }

    public virtual DbSet<TPrivateConversation> TPrivateConversations { get; set; }

    public virtual DbSet<TPrivateMember> TPrivateMembers { get; set; }

    public virtual DbSet<TPrivateMessage> TPrivateMessages { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<VPConversation> VPConversations { get; set; }

    public virtual DbSet<VPConversationMember> VPConversationMembers { get; set; }

    public virtual DbSet<VPConversationMessage> VPConversationMessages { get; set; }

    public virtual DbSet<VUserProfile> VUserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:Database_EM", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TContact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PRIMARY");

            entity.ToTable("t_contact");

            entity.HasIndex(e => new { e.UserId, e.ContactUserId }, "UniqueUserContactUser").IsUnique();

            entity.Property(e => e.ContactUserId).HasComment("联系人用户Id");
            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.UserId).HasComment("此数据所属用户Id");
        });

        modelBuilder.Entity<TFile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PRIMARY");

            entity.ToTable("t_file");

            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileStorageName).HasMaxLength(255);
            entity.Property(e => e.FileType).HasMaxLength(255);
            entity.Property(e => e.OwnerId).HasMaxLength(255);
            entity.Property(e => e.OwnerType).HasMaxLength(255);
        });

        modelBuilder.Entity<TGroupConversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PRIMARY");

            entity.ToTable("t_group_conversation");

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Owner).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
        });

        modelBuilder.Entity<TGroupMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PRIMARY");

            entity.ToTable("t_group_member");

            entity.HasIndex(e => new { e.ConversationId, e.UserId }, "UniqueGCUser").IsUnique();

            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.GroupRemark).HasMaxLength(255);
            entity.Property(e => e.IsAdmin).HasColumnType("int(1)");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.UserRemark).HasMaxLength(255);
        });

        modelBuilder.Entity<TGroupMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("t_group_message");

            entity.Property(e => e.MessageId).HasColumnType("bigint(20)");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.MessageType).HasMaxLength(255);
            entity.Property(e => e.ReplyFor).HasMaxLength(255);
            entity.Property(e => e.SendTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Source).HasMaxLength(255);
        });

        modelBuilder.Entity<TGroupMessageRead>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("t_group_message_read");

            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.ReadTime).HasColumnType("datetime(3)");
        });

        modelBuilder.Entity<TPrivateConversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PRIMARY");

            entity.ToTable("t_private_conversation");

            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
        });

        modelBuilder.Entity<TPrivateMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PRIMARY");

            entity.ToTable("t_private_member");

            entity.HasIndex(e => new { e.ConversationId, e.UserId }, "UniquePCUser").IsUnique();

            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
        });

        modelBuilder.Entity<TPrivateMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("t_private_message");

            entity.Property(e => e.MessageId).HasColumnType("bigint(20)");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.MessageType).HasMaxLength(255);
            entity.Property(e => e.ReadTime).HasColumnType("datetime(3)");
            entity.Property(e => e.ReplyFor).HasMaxLength(255);
            entity.Property(e => e.SendTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Source).HasMaxLength(255);
        });

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("t_user");

            entity.HasIndex(e => e.Emid, "UniqueEMID").IsUnique();

            entity.HasIndex(e => e.Token, "UniqueToken").IsUnique();

            entity.HasIndex(e => e.Username, "UniqueUsername").IsUnique();

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Emid).HasColumnName("EMID");
            entity.Property(e => e.FileToken).HasMaxLength(255);
            entity.Property(e => e.NickName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PublicKey).HasMaxLength(255);
            entity.Property(e => e.Token).HasComment("只是表示在设计表时规定最大长度为255个字符，但实际存储的数据长度可以超过这个限制");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
        });

        modelBuilder.Entity<VPConversation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_p_conversation");

            entity.Property(e => e.ConversationId).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.NewestMessageId).HasColumnType("bigint(20)");
            entity.Property(e => e.OtherMemberId).HasMaxLength(255);
            entity.Property(e => e.OtherUserId).HasMaxLength(255);
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.UnreadCount)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(21)");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        modelBuilder.Entity<VPConversationMember>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_p_conversation_member");

            entity.Property(e => e.ConversationId).HasMaxLength(255);
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.OtherMemberId).HasMaxLength(255);
            entity.Property(e => e.OtherUserId).HasMaxLength(255);
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        modelBuilder.Entity<VPConversationMessage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_p_conversation_message");

            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.ConversationId).HasMaxLength(255);
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.MessageId).HasColumnType("bigint(20)");
            entity.Property(e => e.MessageType).HasMaxLength(255);
            entity.Property(e => e.ReadTime).HasColumnType("datetime(3)");
            entity.Property(e => e.ReplyFor).HasMaxLength(255);
            entity.Property(e => e.SendTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        modelBuilder.Entity<VUserProfile>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_user_profile");

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.Emid)
                .HasMaxLength(255)
                .HasColumnName("EMID");
            entity.Property(e => e.NickName).HasMaxLength(255);
            entity.Property(e => e.PublicKey).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime(3)");
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
