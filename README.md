# MiniSteam 🎮

MiniSteam es una API REST desarrollada como el backend de un sistema de gestión interna para un local de venta de videojuegos. Está pensada para que el personal administrativo y de ventas gestione catálogo, stock y operaciones diarias.

Funcionalidades implementadas (API):
•	CRUD de videojuegos (nombre, género, plataforma, precio).
•	Gestión y actualización de stock tras ventas.
•	Registro de ventas mediante endpoints.
•	Búsquedas y filtros por nombre, género y plataforma.
•	Endpoints para reportes básicos (ventas y stock).
•	Gestión de usuarios y control de acceso con JWT (autenticación) y roles/privilegios.
•	Logging estructurado y centralizado con Serilog.

Rol: Desarrollo del backend, diseño de la API y de la arquitectura en capas.

Tecnologías principales: C# con .NET 9, Serilog (logging), autenticación basada en JWT, SQL (diseño para BD relacional).

Persistencia: la base de datos se crea y mantiene mediante migraciones (EF Core); la solución incluye la configuración y migraciones necesarias para generar la BD al aplicar migraciones.

Estado: Solo la API está implementada; el frontend y la interfaz de usuario no se completaron.
