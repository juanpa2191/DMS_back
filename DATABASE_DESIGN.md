# Diseño de Base de Datos - MemoryKeeper

## Análisis de Requerimientos

### Entidades Principales:
1. **Usuario** - Pepe y sus amigos
2. **Recuerdo** - Memoria principal con estado (Sospecha/Confirmado)
3. **Lugar** - Ubicaciones asociadas a recuerdos
4. **Objeto** - Objetos asociados a recuerdos
5. **Nota** - Notas asociadas a recuerdos
6. **Persona** - Personas asociadas a recuerdos

### Relaciones:
- Un Usuario puede crear múltiples Recuerdos
- Un Recuerdo puede tener múltiples Lugares, Objetos, Notas y Personas
- Cada asociación debe registrar quién creó el elemento y quién lo asoció al recuerdo
- Un Usuario puede confirmar Recuerdos

## Modelo de Datos

### Tabla: Users
```sql
- Id (PK, int, identity)
- Email (nvarchar(255), unique, not null)
- FirstName (nvarchar(100), not null)
- LastName (nvarchar(100), not null)
- PasswordHash (nvarchar(255), not null)
- CreatedAt (datetime2, not null)
- UpdatedAt (datetime2, not null)
- IsActive (bit, not null, default 1)
```

### Tabla: Memories
```sql
- Id (PK, int, identity)
- Title (nvarchar(200), not null)
- Description (nvarchar(max), not null)
- CreatedAt (datetime2, not null) -- Hora de creación
- OccurredAt (datetime2, not null) -- Hora en que sucedió
- Status (int, not null) -- 0: Sospecha, 1: Confirmado
- CreatedByUserId (FK, int, not null)
- ConfirmedByUserId (FK, int, nullable)
- ConfirmedAt (datetime2, nullable)
- UpdatedAt (datetime2, not null)
```

### Tabla: Places
```sql
- Id (PK, int, identity)
- Name (nvarchar(200), not null)
- Description (nvarchar(500), nullable)
- Address (nvarchar(300), nullable)
- Latitude (decimal(10,8), nullable)
- Longitude (decimal(11,8), nullable)
- CreatedByUserId (FK, int, not null)
- CreatedAt (datetime2, not null)
- UpdatedAt (datetime2, not null)
```

### Tabla: Objects
```sql
- Id (PK, int, identity)
- Name (nvarchar(200), not null)
- Description (nvarchar(500), nullable)
- CreatedByUserId (FK, int, not null)
- CreatedAt (datetime2, not null)
- UpdatedAt (datetime2, not null)
```

### Tabla: Notes
```sql
- Id (PK, int, identity)
- Title (nvarchar(200), not null)
- Content (nvarchar(max), not null)
- CreatedByUserId (FK, int, not null)
- CreatedAt (datetime2, not null)
- UpdatedAt (datetime2, not null)
```

### Tabla: People
```sql
- Id (PK, int, identity)
- FirstName (nvarchar(100), not null)
- LastName (nvarchar(100), not null)
- Description (nvarchar(500), nullable)
- CreatedByUserId (FK, int, not null)
- CreatedAt (datetime2, not null)
- UpdatedAt (datetime2, not null)
```

### Tablas de Asociación:

### Tabla: MemoryPlaces
```sql
- Id (PK, int, identity)
- MemoryId (FK, int, not null)
- PlaceId (FK, int, not null)
- AssociatedByUserId (FK, int, not null)
- AssociatedAt (datetime2, not null)
```

### Tabla: MemoryObjects
```sql
- Id (PK, int, identity)
- MemoryId (FK, int, not null)
- ObjectId (FK, int, not null)
- AssociatedByUserId (FK, int, not null)
- AssociatedAt (datetime2, not null)
```

### Tabla: MemoryNotes
```sql
- Id (PK, int, identity)
- MemoryId (FK, int, not null)
- NoteId (FK, int, not null)
- AssociatedByUserId (FK, int, not null)
- AssociatedAt (datetime2, not null)
```

### Tabla: MemoryPeople
```sql
- Id (PK, int, identity)
- MemoryId (FK, int, not null)
- PersonId (FK, int, not null)
- AssociatedByUserId (FK, int, not null)
- AssociatedAt (datetime2, not null)
```

## Índices Recomendados

```sql
-- Búsqueda de recuerdos por usuario
CREATE INDEX IX_Memories_CreatedByUserId ON Memories(CreatedByUserId);

-- Búsqueda de recuerdos por estado
CREATE INDEX IX_Memories_Status ON Memories(Status);

-- Búsqueda de recuerdos por fecha de ocurrencia
CREATE INDEX IX_Memories_OccurredAt ON Memories(OccurredAt);

-- Búsqueda full-text en recuerdos
CREATE FULLTEXT INDEX ON Memories(Title, Description);

-- Índices para las tablas de asociación
CREATE INDEX IX_MemoryPlaces_MemoryId ON MemoryPlaces(MemoryId);
CREATE INDEX IX_MemoryObjects_MemoryId ON MemoryObjects(MemoryId);
CREATE INDEX IX_MemoryNotes_MemoryId ON MemoryNotes(MemoryId);
CREATE INDEX IX_MemoryPeople_MemoryId ON MemoryPeople(MemoryId);
```

## Funcionalidades Cubiertas

✓ Identificación de usuarios (Pepe y amigos)
✓ Creación de recuerdos con hora de creación y ocurrencia
✓ Estados de recuerdo (Sospecha/Confirmado)
✓ Asociación de lugares, objetos, notas y personas
✓ Registro de creadores y asociadores
✓ Confirmación de recuerdos
✓ Búsqueda por palabras clave (Full-text search)
✓ Listado de recuerdos por usuario
✓ Listado de elementos asociados a recuerdos