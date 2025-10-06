export const QUERY_KEYS = {
    ME: ["me"] as const,
    STATIONS: ["stations"] as const,
    VEHICLE_SEGMENTS: ["vehicleSegments"] as const,
    VEHICLE_MODELS: ["vehicleModels"] as const,
    VEHICLE_MODEL: (id: string) => ["vehicleModel", id] as const
    // VEHICLE: {
    //     ALL: ["vehicles"] as const,
    //     BY_ID: (id: string) => ["vehicles", id] as const
    // }
}

// added: string-based keys for new query hooks
// export const QUERY_KEY = {
//     // AUTH: "auth",
//     // PROFILE: "profile",
//     // STATION: "station",
//     // VEHICLE_SEGMENT: "vehicleSegment",
//     // RENTAL_CONTRACTS: "rentalContracts"
// } as const
