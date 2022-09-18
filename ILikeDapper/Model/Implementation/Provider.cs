using Dapper;
using ILikeDapper.Model.Interface;
using Npgsql;
using System.Data;

namespace ILikeDapper.Model.Implementation
{
    public class Provider
    {
        private readonly string _connectionString = "Host=localhost;Username=postgres;Password=123;Database=parameters";
        private readonly string _groupTable = "groups";
        private readonly string _parameterTable = "parameterTable";
        private readonly string _attributeTable = "attributeTable";

        private readonly string _boolAttributeColum = "boolAttribute";
        private readonly string _intAttributeColumn = "intAttribute";
        private readonly string _doubleAttributeColumn = "doubleAttribute";
        private readonly string _stringAttriubteColumn = "stringAttribute";

        private readonly string _idParentColumn = "id_parent";
        private readonly string _idColumn = "id";
        private readonly string _nameColumn = "name";
        private readonly string _typeColumn = "type";

        public void CreateTables()
        {
            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            //create group table
            cmd.CommandText = @$"DROP TABLE IF EXISTS {_groupTable}";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @$"CREATE TABLE {_groupTable}
                                ({_idColumn} SERIAL PRIMARY KEY, 
                                {_idParentColumn} integer ,
                                {_nameColumn} VARCHAR(255),
                                {_typeColumn} integer)";
            cmd.ExecuteNonQuery();

            //create parameter table
            cmd.CommandText = @$"DROP TABLE IF EXISTS {_parameterTable}";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @$"CREATE TABLE {_parameterTable}
                                ({_idColumn} SERIAL PRIMARY KEY,
                                {_idParentColumn} integer ,
                                {_nameColumn} VARCHAR(255),
                                {_typeColumn} integer)";
            cmd.ExecuteNonQuery();

            //create attribute table
            cmd.CommandText = @$"DROP TABLE IF EXISTS {_attributeTable}";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @$"CREATE TABLE {_attributeTable}
                                ({_idColumn} SERIAL PRIMARY KEY, 
                                {_idParentColumn} integer ,
                                {_nameColumn} VARCHAR(255), 
                                {_intAttributeColumn} integer, 
                                {_doubleAttributeColumn} double precision, 
                                {_boolAttributeColum} boolean,
                                {_stringAttriubteColumn} VARCHAR(255),
                                {_typeColumn} integer)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("Table groups created");
        }

        #region update
        public async Task UpdateGroup(IGroup group)
        {
            string sql = @$"UPDATE {_groupTable} 
                            SET {_idParentColumn} = @id_parent, 
                                {_nameColumn} = @name,
                                {_typeColumn} = @type 
                            WHERE {_idColumn} = @Id;";

            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                await db.ExecuteAsync(sql, new { id_parent = group?.Parent?.Id, name = group?.Name, type=group?.Type, Id = group?.Id });
            }
        }
        #endregion

        #region insert
        public async Task InsertExtendedAttribute(IExtendedAttribute attribute)
        {
            if (attribute == null) return;
            var p = new DynamicParameters();

            string sql = "";

            int createdId = -1;

            p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@name", attribute.Name, DbType.String);
            p.Add("@id_parent", attribute.Parent?.Id);
            p.Add("@type", attribute.Type);

            p.Add($"@{_doubleAttributeColumn}", attribute.DoulbeAttribute, DbType.Double);
            p.Add($"@{_intAttributeColumn}", attribute.IntAttribute, DbType.Int32);
            p.Add($"@{_boolAttributeColum}", attribute.BoolAttribute, DbType.Boolean);
            p.Add($"@{_stringAttriubteColumn}", attribute.StringAttribute, DbType.String);

            sql = $@"insert into {_attributeTable} 
                    ({_nameColumn}, 
                     {_idParentColumn},
                     {_typeColumn},
                     {_doubleAttributeColumn},
                     {_intAttributeColumn},
                     {_boolAttributeColum},
                     {_stringAttriubteColumn}) 
             values (@{_nameColumn}, 
                     @{_idParentColumn},
                     @{_typeColumn},
                     @{_doubleAttributeColumn},
                     @{_intAttributeColumn},
                     @{_boolAttributeColum},
                     @{_stringAttriubteColumn}) 
                     RETURNING id;";

            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, p);
                createdId = p.Get<int>("@id");
            }

            attribute.Id = createdId;
        }
        public async Task InsertParameter(IParameter parameter)
        {
            var p = new DynamicParameters();

            string sql = "";

            int createdId = -1;

            p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@name", "holdrio");
            p.Add("@id_parent", parameter.Parent?.Id);
            p.Add("@type", parameter.Type);

            sql = $@"insert into  {_parameterTable} 
                                ({_nameColumn}, 
                                 {_idParentColumn},
                                 {_typeColumn}) 
                                values (@name, 
                                @id_parent, 
                                @type) 
                                RETURNING id;";

            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, p);
                createdId = p.Get<int>("@id");
            }

            parameter.Id = createdId;
        }
        public async Task InsertGroup(IGroup group)
        {
            var p = new DynamicParameters();
            var parentId = group.Parent?.Id;
            string sql = "";

            int createdId = -1;

            if (parentId != null)
            {
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@name", group.Name);
                p.Add("@id_parent", parentId.Value);
                p.Add("@type", group.Type);

                sql = $@"insert into  {_groupTable} 
                                ({_nameColumn}, {_idParentColumn}, {_typeColumn}) 
                                values (@name, @id_parent, @type) 
                                RETURNING id;";
            }
            else
            {

                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@name", group.Name);
                p.Add("@type", group.Type);

                sql = $@"insert into  {_groupTable} 
                                ({_nameColumn},{_typeColumn}) 
                                values (@name, @type) 
                                RETURNING id;";
            }


            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {

                await connection.ExecuteAsync(sql, p);
                createdId = p.Get<int>("@id");
            }

            group.Id = createdId;

        }
        #endregion

        #region get
        public async Task<IEnumerable<SimpleParameterGroup>> GetAllGroups()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string commandText = $"SELECT * FROM {_groupTable}";
                return await connection.QueryAsync<SimpleParameterGroup>(commandText);

            }

        }
        public async Task<IEnumerable<SimpleParameterGroup>> GetAllSubGroupsForGroup(int groupId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string commandText = $"SELECT * FROM {_groupTable} Where {_idParentColumn} = @pId";
                return await connection.QueryAsync<SimpleParameterGroup>(commandText, new { pId = groupId });

            }

        }
        public async Task<IEnumerable<SimpleParameter>> GetAllParametersForGroup(int groupId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string commandText = $"SELECT * FROM {_parameterTable} Where {_idParentColumn} = @pId";
                return await connection.QueryAsync<SimpleParameter>(commandText, new { pId = groupId });

            }
        }
        public async Task<SimpleParameterGroup> GetGroup(int groupId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string commandText = $"SELECT * FROM {_groupTable} Where {_idColumn} = @key";
                return await connection.QueryFirstAsync<SimpleParameterGroup>(commandText, new { key = groupId });

            }
        }
        public async Task<SimpleParameterGroup> GetGroupWithSubGroupsAndParameter(int groupId)
        {

            var group = await GetGroup(groupId);
            var suGroups = await GetAllSubGroupsForGroup(groupId);
            foreach (var sub in suGroups)
            {
                group.Groups.Add(sub);
            }
            var parameters = await GetAllParametersForGroup(groupId);
            foreach (var param in parameters)
            {
                group.Parameters.Add(param);
            }
            return group;
        }
        public async Task<IEnumerable<IExtendedAttribute>> GetAllAttributesForParameter(int parameterId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var commandText = $@"SELECT 
                                    {_idColumn} AS StringAttribute, 
                                    {_idParentColumn} AS ParentId, 
                                    {_nameColumn} AS Name, 
                                    {_stringAttriubteColumn} AS StringAttribute,  
                                    {_typeColumn} AS Type  
                                    FROM {_attributeTable} 
                                    Where {_idParentColumn} = @pId";
                return await connection.QueryAsync<ExtendedAttribute>(commandText, new { pId = parameterId });

            }

        }
        
        #endregion


    }
}
