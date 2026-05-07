#nullable disable
namespace Demo.Data.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(LibraryDbContext))]
[Migration("20260416150253_Init")]
partial class Init
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.6")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("SoftUniLibrary.Data.Models.Author", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.HasKey("Id");

                b.ToTable("Authors");
            });

        modelBuilder.Entity("SoftUniLibrary.Data.Models.Book", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("AuthorId")
                    .HasColumnType("int");

                b.Property<bool>("IsBorrowed")
                    .HasColumnType("bit");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<int>("Year")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("AuthorId");

                b.ToTable("Books");
            });

        modelBuilder.Entity("SoftUniLibrary.Data.Models.Book", b =>
            {
                b.HasOne("SoftUniLibrary.Data.Models.Author", "Author")
                    .WithMany("Books")
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Author");
            });

        modelBuilder.Entity("SoftUniLibrary.Data.Models.Author", b =>
            {
                b.Navigation("Books");
            });
#pragma warning restore 612, 618
    }
}
