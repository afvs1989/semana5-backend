# Backend — Vehículos API (.NET 10)

API REST en **.NET 10** con arquitectura en capas para el CRUD de vehículos y autenticación por cookies.

## Estructura

```
backend/
├── src/
│   ├── VehiculosApi.Api/            # Controllers, Program.cs, middleware
│   ├── VehiculosApi.Application/    # DTOs, interfaces, servicios de negocio
│   ├── VehiculosApi.Domain/         # Entidades
│   └── VehiculosApi.Infrastructure/ # EF Core, repositorios, auth
├── tests/
│   └── VehiculosApi.Tests/          # Pruebas unitarias
├── VehiculosApi.sln
└── VehiculosApi.http                # Colección de requests HTTP
```

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server en `localhost:1433` con base de datos `semana5`

## Configuración

Archivo: `src/VehiculosApi.Api/appsettings.json`

| Clave | Descripción |
|---|---|
| `ConnectionStrings:DefaultConnection` | Cadena de conexión a SQL Server |
| `Cors:AllowedOrigin` | Origen permitido del frontend (`http://localhost:4200`) |
| `CookieSettings:Secure` | `false` en desarrollo HTTP; `true` en producción con HTTPS |

### Variables de entorno (recomendado en producción)

```bash
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=semana5;User Id=sa;Password=TuPasswordSeguro123!;TrustServerCertificate=True"
export Cors__AllowedOrigin="http://localhost:4200"
export CookieSettings__Secure="false"
```

> No suba contraseñas al repositorio. Use User Secrets o variables de entorno.

## Ejecutar

```bash
cd backend
dotnet restore
dotnet run --project src/VehiculosApi.Api
```

La API queda en `http://localhost:5121`.

Las migraciones de Entity Framework se aplican automáticamente al iniciar.

## Credenciales de prueba

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin` | `Admin123!` | Administrador |

## Entidad Vehículo

Campos: Id, Marca, Modelo, Anio, Color, Placa, Vin, Kilometraje, TipoCombustible, Precio, Estado, FechaRegistro.

## Endpoints principales

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Iniciar sesión | Público |
| POST | `/api/auth/logout` | Cerrar sesión | Cookie |
| GET | `/api/auth/status` | Estado de sesión | Público |
| GET | `/api/csrf/token` | Token CSRF | Público |
| GET | `/api/vehiculos` | Listar vehículos | Cookie |
| POST | `/api/vehiculos` | Crear vehículo | Admin |
| PUT | `/api/vehiculos/{id}` | Actualizar | Admin |
| DELETE | `/api/vehiculos/{id}` | Eliminar | Admin |

## Seguridad implementada

- Cookie de sesión `VehiculosSession` (HttpOnly, SameSite=Strict, Secure configurable)
- Regeneración de sesión al login
- Protección CSRF con header `X-XSRF-TOKEN`
- Códigos HTTP: 200, 400, 401, 403

## Pruebas

```bash
dotnet test
```

También puede probar la API con `VehiculosApi.http` (REST Client en VS Code).
