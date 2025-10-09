import { VehicleModelViewRes } from "@/models/vehicle/schema/response"
import { create } from "zustand"
import { persist, createJSONStorage } from "zustand/middleware"

interface BookingState {
    stationId?: string
    segmentId?: string
    startDate?: string
    endDate?: string
    filteredVehicleModels: VehicleModelViewRes[]
}

interface BookingActions {
    setStationId: (id?: string) => void
    setSegmentId: (id?: string) => void
    setStartDate: (date?: string) => void
    setEndDate: (date?: string) => void
    setBookingFilter: (
        station?: string,
        segment?: string,
        startDate?: string,
        endDate?: string
    ) => void
    clearBookingFilter: () => void
    setFilteredVehicleModels: (filteredVehicleModels: VehicleModelViewRes[]) => void
}

export const useBookingFilterStore = create<BookingState & BookingActions>()(
    persist(
        (set) => ({
            // --- State ---
            stationId: undefined,
            segmentId: undefined,
            startDate: undefined,
            endDate: undefined,
            filteredVehicleModels: [],

            // --- Actions ---
            setStationId: (id) => set({ stationId: id }),
            setSegmentId: (id) => set({ segmentId: id }),
            setStartDate: (date) => set({ startDate: date }),
            setEndDate: (date) => set({ endDate: date }),

            setBookingFilter: (station, segment, startDate, endDate) =>
                set({
                    stationId: station,
                    segmentId: segment,
                    startDate: startDate,
                    endDate: endDate
                }),

            clearBookingFilter: () =>
                set({
                    stationId: undefined,
                    segmentId: undefined,
                    startDate: undefined,
                    endDate: undefined,
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
