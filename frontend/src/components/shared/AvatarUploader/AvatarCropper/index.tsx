"use client"

import React, { useCallback, useState } from "react"
import Cropper from "react-easy-crop"
import { SliderStyled } from "@/components/styled/SliderStyled"

interface AvatarCropperProps {
    imgSrc: string
    setCroppedAreaPixels: (area: any) => void
    // setImgSrc: (src: string | null) => void
}

export function AvatarCropper({ imgSrc, setCroppedAreaPixels }: AvatarCropperProps) {
    const [crop, setCrop] = useState({ x: 0, y: 0 })
    const [zoom, setZoom] = useState(1)
    const [minZoom, setMinZoom] = useState(1)
    const maxZoom = 10

    const handleCropComplete = useCallback(
        (_: any, croppedAreaPixels: any) => {
            setCroppedAreaPixels(croppedAreaPixels)
        },
        [setCroppedAreaPixels]
    )

    const handleMediaLoaded = useCallback((mediaSize: { width: number; height: number }) => {
        const aspect = 1 // vì aspect crop là 1:1
        const { width, height } = mediaSize
        const imageAspect = width / height
        let newMinZoom = 1

        if (imageAspect > aspect) {
            // ảnh rộng hơn => fit theo chiều cao
            newMinZoom = height / width
        } else {
            // ảnh cao hơn => fit theo chiều rộng
            newMinZoom = width / height
        }

        // Zoom vừa đủ để phủ kín khung crop
        const adjustedZoom = 1 / newMinZoom
        setMinZoom(adjustedZoom)
        setZoom(adjustedZoom)
    }, [])

    return (
        <div className="relative w-[300px] h-[300px] bg-gray-900">
            <Cropper
                image={imgSrc}
                crop={crop}
                zoom={zoom}
                aspect={1} // 1:1
                cropSize={{ width: 300, height: 300 }}
                restrictPosition={true}
                onCropChange={setCrop}
                onZoomChange={setZoom}
                onCropComplete={handleCropComplete}
                cropShape="round"
                minZoom={minZoom}
                maxZoom={maxZoom}
                onMediaLoaded={handleMediaLoaded}
            />
            <div className="absolute top-0 right-[-30px] h-full flex items-center">
                <SliderStyled
                    minValue={minZoom}
                    maxValue={maxZoom}
                    step={0.1}
                    value={zoom}
                    orientation="vertical"
                    onChange={(value) => setZoom(value as number)}
                    className="h-[250px]"
                />
            </div>
        </div>
    )
}
