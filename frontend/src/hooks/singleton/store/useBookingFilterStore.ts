import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"
import { create } from "zustand"
import { persist, createJSONStorage } from "zustand/middleware"

interface BookingState {
    stationId: string | null
    segmentId: string | null
    startDate: string | null
    endDate: string | null
    filteredVehicleModels: VehicleModelViewRes[]
}

interface BookingActions {
    setStationId: (id: string | null) => void
    setSegmentId: (id: string | null) => void
    setStartDate: (date: string | null) => void
    setEndDate: (date: string | null) => void
    setBookingFilter: (
        station: string | null,
        segment: string | null,
        start: string | null,
        end: string | null
    ) => void
    clearBookingFilter: () => void
    setFilteredVehicleModels: (filteredVehicleModels: VehicleModelViewRes[]) => void
}

export const useBookingFilterStore = create<BookingState & BookingActions>()(
    persist(
        (set) => ({
            // --- State ---
            stationId: null,
            segmentId: null,
            startDate: null,
            endDate: null,
            filteredVehicleModels: [],

            // --- Actions ---
            setStationId: (id) => set({ stationId: id }),
            setSegmentId: (id) => set({ segmentId: id }),
            setStartDate: (date) => set({ startDate: date }),
            setEndDate: (date) => set({ endDate: date }),

            setBookingFilter: (station, segment, start, end) =>
                set({
                    stationId: station,
                    segmentId: segment,
                    startDate: start,
                    endDate: end
                }),

            clearBookingFilter: () =>
                set({
                    stationId: null,
                    segmentId: null,
                    startDate: null,
                    endDate: null,
                    filteredVehicleModels: []
                }),

            setFilteredVehicleModels: (filteredVehicleModels) => set({ filteredVehicleModels })
        }),
        {
            name: "booking_filter_storage",
            storage: createJSONStorage(() => sessionStorage),
            partialize: (state) => ({
                stationId: state.stationId,
                segmentId: state.segmentId,
                startDate: state.startDate,
                endDate: state.endDate
            })
        }
    )
)

// ----------------------
// Selectors (để tối ưu render)
// ----------------------
// export const useBookingInfo = () => {
//     const selector = useShallow((s: BookingState) => ({
//         station: s.station,
//         start: s.start,
//         end: s.end
//     }))
//     return useBookingFilterStore(selector)
// }

// export const useFilteredVehicles = () => useBookingFilterStore((s) => s.filteredVehicles)

// export const useBookingActions = () =>
//     useBookingFilterStore(
//         (s) => ({
//             setBookingFilter: s.setBookingFilter,
//             setFilteredVehicles: s.setFilteredVehicles,
//             clearBookingFilter: s.clearBookingFilter
//         }),
//         shallow
//     )

// const savedBooking =
//     typeof window !== "undefined" ? JSON.parse(sessionStorage.getItem("booking") || "{}") : {}

// export const useBookingFilterStore = create<BookingState>((set) => ({
//     station: savedBooking.station ?? null,
//     start: savedBooking.start ?? null,
//     end: savedBooking.end ?? null,

//     setBooking: (data) => {
//         set((state) => {
//             const newState = { ...state, ...data }
//             if (typeof window !== "undefined") {
//                 sessionStorage.setItem("booking", JSON.stringify(newState))
//             }
//             return newState
//         })
//     },

//     clearBookingFilter: () => {
//         if (typeof window !== "undefined") {
//             sessionStorage.removeItem("booking")
//         }
//         set({ station: null, start: null, end: null })
//     }
// }))
