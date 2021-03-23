namespace Assets.CBC.Scripts.Terrain
{
    using UnityEngine;

    public static class TerrainUtils
    {
        /// <summary>
        /// Terrain data in held in a 2-dimensional int array. This converts a world pos into the accessor for that array
        /// </summary>
        /// <param name="terrain">terrain to examine</param>
        /// <param name="worldPosition">world position to convert</param>
        /// <returns>(x,y) coords into the Terrain Data array</returns>
        public static (int x, int y) GetTerrainCoordsFromWorldPosition(Terrain terrain, Vector3 worldPosition)
        {
            var terrainPosition = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;
            var terrainSize = terrainData.size;
            float relativeHitTerX = (worldPosition.x - terrainPosition.x) / terrainSize.x;
            float relativeHitTerZ = (worldPosition.z - terrainPosition.z) / terrainSize.z;

            float relativeTerCoordX = terrainData.heightmapResolution * relativeHitTerX;
            float relativeTerCoordZ = terrainData.heightmapResolution * relativeHitTerZ;

            int hitPointTerX = Mathf.FloorToInt(relativeTerCoordX);
            int hitPointTerZ = Mathf.FloorToInt(relativeTerCoordZ);

            // Yes, Z as X, X as Y
            return (hitPointTerZ, hitPointTerX);
        }

        /// <summary>
        /// Get height information from the Terrain data 2D array
        /// </summary>
        /// <param name="terrainData">Terrain to examine</param>
        /// <param name="heights">2D height array from terrain</param>
        /// <param name="coords">(x,y) array accessor</param>
        /// <returns>the raw data height in the array and the converted world height</returns>
        public static (float dataHeight, float worldHeight) GetHeightDataFromCoords(TerrainData terrainData, float[,] heights, (int x, int y) coords)
        {
            if (coords.x > heights.GetLength(0) || coords.y > heights.GetLength(1))
            {
                Debug.LogError($"{coords} is out of range");
                return (0f, 0f);
            }

            var dataHeight = heights[coords.x, coords.y];
            var worldHeight = dataHeight * terrainData.size.y;
            return (dataHeight, worldHeight);
        }

    }
}