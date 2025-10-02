'use client'

import { RegisterReceiveForm } from '@/components'
import dynamic from 'next/dynamic'

// const RegisterReceiveCar = dynamic(
//   () => import('@/components/shared/RegisterReceiveForm'),
//   { ssr: false }
// )

export default function RegisterCarPage() {
  return <RegisterReceiveForm />
}