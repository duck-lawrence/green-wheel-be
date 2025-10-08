"use client"

import React from "react"
import { CreateRentalContractForm } from "@/components"

// const RegisterReceiveCar = dynamic(
//   () => import('@/components/shared/CreateRentalContractForm'),
//   { ssr: false }
// )

export default function CreateRentalContractPage() {
    return <CreateRentalContractForm />
}
