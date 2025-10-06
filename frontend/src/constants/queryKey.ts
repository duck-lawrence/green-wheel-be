export const QUERY_KEYS = {
    ME: ["me"] as const,
    STATIONS: ["stations"] as const,
    VEHICLE_SEGMENTS: ["vehicleSegments"] as const,
    VEHICLE_MODELS: ["vehicleModels"] as const
    // POSTS: ["posts"] as const,
    // POST: (id: number) => ["post", id] as const
}

// added: string-based keys for new query hooks
export const QUERY_KEY = {
    AUTH: "auth",
    PROFILE: "profile",
    STATION: "station",
    VEHICLE_SEGMENT: "vehicleSegment",
    RENTAL_CONTRACTS: "rentalContracts"
} as const
