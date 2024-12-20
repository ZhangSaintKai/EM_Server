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
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Database_EM");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TContact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PRIMARY_Contact");

            entity.ToTable("t_contact");

            entity.HasIndex(e => new { e.UserId, e.ContactUserId }, "UniqueUserContactUser").IsUnique();

            entity.Property(e => e.ContactUserId).HasComment("联系人用户Id");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasComment("此数据所属用户Id");
        });

        modelBuilder.Entity<TFile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PRIMARY_File");

            entity.ToTable("t_file");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileStorageName).HasMaxLength(255);
            entity.Property(e => e.FileType).HasMaxLength(255);
            entity.Property(e => e.OwnerId).HasMaxLength(255);
            entity.Property(e => e.OwnerType).HasMaxLength(255);
        });

        modelBuilder.Entity<TGroupConversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PRIMARY_GroupConversation");

            entity.ToTable("t_group_conversation");

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Owner).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TGroupMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PRIMARY_GroupMember");

            entity.ToTable("t_group_member");

            entity.HasIndex(e => new { e.ConversationId, e.UserId }, "UniqueGCUser")
                .IsUnique()
                .HasFilter("([UserId] IS NOT NULL)");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.GroupRemark).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserRemark).HasMaxLength(255);
        });

        modelBuilder.Entity<TGroupMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY_GroupMessage");

            entity.ToTable("t_group_message");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.MessageType).HasMaxLength(255);
            entity.Property(e => e.ReplyFor).HasMaxLength(255);
            entity.Property(e => e.SendTime).HasColumnType("datetime");
            entity.Property(e => e.Signature).HasColumnType("text");
            entity.Property(e => e.Source).HasMaxLength(255);
        });

        modelBuilder.Entity<TGroupMessageRead>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY_GroupMessageRead");

            entity.ToTable("t_group_message_read");

            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.ReadTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TPrivateConversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PRIMARY_PrivateConversation ");

            entity.ToTable("t_private_conversation");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TPrivateMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PRIMARY_PrivateMember");

            entity.ToTable("t_private_member");

            entity.HasIndex(e => new { e.ConversationId, e.UserId }, "UniquePCUser").IsUnique();

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TPrivateMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY_PrivateMessage");

            entity.ToTable("t_private_message");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.MessageType).HasMaxLength(255);
            entity.Property(e => e.ReadTime).HasColumnType("datetime");
            entity.Property(e => e.ReplyFor).HasMaxLength(255);
            entity.Property(e => e.SendTime).HasColumnType("datetime");
            entity.Property(e => e.Signature).HasColumnType("text");
            entity.Property(e => e.Source).HasMaxLength(255);
        });

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY_User");

            entity.ToTable("t_user");

            entity.HasIndex(e => e.Emid, "UniqueEMID").IsUnique();

            entity.HasIndex(e => e.Token, "UniqueToken")
                .IsUnique()
                .HasFilter("([Token] IS NOT NULL)");

            entity.HasIndex(e => e.Username, "UniqueUsername").IsUnique();

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Emid).HasColumnName("EMID");
            entity.Property(e => e.FileToken).HasMaxLength(255);
            entity.Property(e => e.NickName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PublicKey).HasColumnType("text");
            entity.Property(e => e.Token).HasComment("只是表示在设计表时规定最大长度为255个字符，但实际存储的数据长度可以超过这个限制");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<VPConversation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_p_conversation");

            entity.Property(e => e.ConversationId).HasMaxLength(450);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasMaxLength(450);
            entity.Property(e => e.OtherMemberId).HasMaxLength(450);
            entity.Property(e => e.OtherUserId).HasMaxLength(450);
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<VPConversationMember>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_p_conversation_member");

            entity.Property(e => e.ConversationId).HasMaxLength(450);
            entity.Property(e => e.MemberId).HasMaxLength(450);
            entity.Property(e => e.OtherMemberId).HasMaxLength(450);
            entity.Property(e => e.OtherUserId).HasMaxLength(450);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<VPConversationMessage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_p_conversation_message");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.ConversationId).HasMaxLength(450);
            entity.Property(e => e.MemberId).HasMaxLength(255);
            entity.Property(e => e.MessageType).HasMaxLength(255);
            entity.Property(e => e.ReadTime).HasColumnType("datetime");
            entity.Property(e => e.ReplyFor).HasMaxLength(255);
            entity.Property(e => e.SendTime).HasColumnType("datetime");
            entity.Property(e => e.Signature).HasColumnType("text");
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<VUserProfile>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_user_profile");

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Emid)
                .HasMaxLength(450)
                .HasColumnName("EMID");
            entity.Property(e => e.NickName).HasMaxLength(255);
            entity.Property(e => e.PublicKey).HasColumnType("text");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
