'use client'

import dynamic from 'next/dynamic'

const RegisterReceiveCar = dynamic(
  () => import('@/components/register_recieve_car'),
  { ssr: false }
)

export default function RegisterCarPage() {
  return <RegisterReceiveCar />
}