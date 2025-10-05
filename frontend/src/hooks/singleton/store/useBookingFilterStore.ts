// src/store/useBookingFilterStore.ts
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"
// import { shallow } from "@/utils/helpers/shallow"
import { create } from "zustand"
import { useShallow } from "zustand/react/shallow"
type BookingState = {
    station: string | null
    start: string | null
    end: string | null
    filteredVehicleModels: VehicleModelViewRes[]
    // setBooking: (data: Partial<BookingState>) => void
    // clearBookingFilter: () => void
}

interface BookingActions {
    setBookingFilter: (station: string, start: string, end: string) => void
    clearBookingFilter: () => void
    setFilteredVehicleModels: (vehicleModels: VehicleModelViewRes[]) => void
}

export const useBookingFilterStore = create<BookingState & BookingActions>((set) => ({
    station: null,
    start: null,
    end: null,
    filteredVehicleModels: [],

    setBookingFilter: (station, start, end) => set({ station, start, end }),

    clearBookingFilter: () =>
        set({
            station: null,
            start: null,
            end: null,
            filteredVehicleModels: []
        }),

    setFilteredVehicleModels: (vehicleModels) => set({ filteredVehicleModels: vehicleModels })
}))

// ----------------------
// Selectors (để tối ưu render)
// ----------------------
export const useBookingInfo = () => {
    const selector = useShallow((s: BookingState) => ({
        station: s.station,
        start: s.start,
        end: s.end
    }))
    return useBookingFilterStore(selector)
}

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
