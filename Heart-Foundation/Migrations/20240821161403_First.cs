using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Heart_Foundation.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "seguridad");

            migrationBuilder.EnsureSchema(
                name: "solicitud");

            migrationBuilder.CreateTable(
                name: "institucion",
                schema: "solicitud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    UbicacionFileLogo = table.Column<string>(type: "text", nullable: false),
                    AreaEmpresa = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institucion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roleclaims",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleclaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "solicitud",
                schema: "solicitud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstadoSolicitud = table.Column<int>(type: "integer", nullable: false),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: false),
                    Motivo = table.Column<string>(type: "text", nullable: false),
                    NumeroSolicitud = table.Column<string>(type: "text", nullable: false),
                    idpersona = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitud", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudUsuario",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    NumeroTelefonico = table.Column<string>(type: "text", nullable: false),
                    Mensaje = table.Column<string>(type: "text", nullable: false),
                    TipoUsuario = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "representante",
                schema: "solicitud",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    Cargo = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_representante", x => x.id);
                    table.ForeignKey(
                        name: "FK_representante_institucion_id",
                        column: x => x.id,
                        principalSchema: "solicitud",
                        principalTable: "institucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "etapa",
                schema: "solicitud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstadoEtapa = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinal = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorEtapa = table.Column<int>(type: "integer", nullable: false),
                    idsolicitud = table.Column<long>(type: "bigint", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etapa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_etapa_solicitud_idsolicitud",
                        column: x => x.idsolicitud,
                        principalSchema: "solicitud",
                        principalTable: "solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expediente",
                schema: "solicitud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nota = table.Column<string>(type: "text", nullable: false),
                    UbicacionFile = table.Column<string>(type: "text", nullable: false),
                    ExtencionnFile = table.Column<string>(type: "text", nullable: false),
                    idsolicitud = table.Column<long>(type: "bigint", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expediente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expediente_solicitud_idsolicitud",
                        column: x => x.idsolicitud,
                        principalSchema: "solicitud",
                        principalTable: "solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "claims",
                schema: "seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: false),
                    ClaimValue = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_claims_usuarios_UserId",
                        column: x => x.UserId,
                        principalSchema: "seguridad",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cuentas",
                schema: "seguridad",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuentas", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_cuentas_usuarios_LoginProvider",
                        column: x => x.LoginProvider,
                        principalSchema: "seguridad",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_roles",
                schema: "seguridad",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_usuario_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "seguridad",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_roles_usuarios_UserId",
                        column: x => x.UserId,
                        principalSchema: "seguridad",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "solicituxrepresentante",
                schema: "solicitud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSolicitud = table.Column<long>(type: "bigint", nullable: false),
                    IdRepresentante = table.Column<long>(type: "bigint", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicituxrepresentante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_solicituxrepresentante_representante_IdRepresentante",
                        column: x => x.IdRepresentante,
                        principalSchema: "solicitud",
                        principalTable: "representante",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicituxrepresentante_solicitud_IdSolicitud",
                        column: x => x.IdSolicitud,
                        principalSchema: "solicitud",
                        principalTable: "solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nota",
                schema: "solicitud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idetapa = table.Column<long>(type: "bigint", nullable: false),
                    TipoNota = table.Column<int>(type: "integer", nullable: false),
                    Mensaje = table.Column<string>(type: "text", nullable: false),
                    FechaHoraMensje = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_nota_etapa_idetapa",
                        column: x => x.idetapa,
                        principalSchema: "solicitud",
                        principalTable: "etapa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_claims_UserId",
                schema: "seguridad",
                table: "claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_etapa_idsolicitud",
                schema: "solicitud",
                table: "etapa",
                column: "idsolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_expediente_idsolicitud",
                schema: "solicitud",
                table: "expediente",
                column: "idsolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_nota_idetapa",
                schema: "solicitud",
                table: "nota",
                column: "idetapa");

            migrationBuilder.CreateIndex(
                name: "IX_solicituxrepresentante_IdRepresentante",
                schema: "solicitud",
                table: "solicituxrepresentante",
                column: "IdRepresentante");

            migrationBuilder.CreateIndex(
                name: "IX_solicituxrepresentante_IdSolicitud",
                schema: "solicitud",
                table: "solicituxrepresentante",
                column: "IdSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_roles_RoleId",
                schema: "seguridad",
                table: "usuario_roles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "claims",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "cuentas",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "expediente",
                schema: "solicitud");

            migrationBuilder.DropTable(
                name: "nota",
                schema: "solicitud");

            migrationBuilder.DropTable(
                name: "roleclaims",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "SolicitudUsuario");

            migrationBuilder.DropTable(
                name: "solicituxrepresentante",
                schema: "solicitud");

            migrationBuilder.DropTable(
                name: "usuario_roles",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "etapa",
                schema: "solicitud");

            migrationBuilder.DropTable(
                name: "representante",
                schema: "solicitud");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "solicitud",
                schema: "solicitud");

            migrationBuilder.DropTable(
                name: "institucion",
                schema: "solicitud");
        }
    }
}
