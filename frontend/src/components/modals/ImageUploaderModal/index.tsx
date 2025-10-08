"use client"

import React, { useCallback, useState } from "react"
import { ModalStyled, ButtonStyled } from "@/components/styled"
import { ModalContent, ModalBody, ModalHeader, Spinner } from "@heroui/react"
import { getCroppedImage } from "@/utils/helpers/image"
import { ImageCropper } from "@/components/shared"
import { useTranslation } from "react-i18next"

type ImageUploaderModalProps = {
    isOpen: boolean
    onOpenChange: () => void
    onClose: () => void
    imgSrc: string | null
    setImgSrc: (src: string | null) => void
    aspect?: number
    cropShape?: "rect" | "round"
    cropSize: { width: number; height: number }
    label?: string
    uploadFn: (formData: FormData) => Promise<any>
}

export function ImageUploaderModal({
    isOpen,
    onOpenChange,
    onClose,
    imgSrc,
    setImgSrc,
    uploadFn,
    aspect = 1,
    cropShape = "rect",
    cropSize,
    label
}: ImageUploaderModalProps) {
    const { t } = useTranslation()
    const [croppedAreaPixels, setCroppedAreaPixels] = useState<any>(null)
    const [isUploading, setIsUploading] = useState(false)

    const handleUpload = useCallback(async () => {
        if (!imgSrc || !croppedAreaPixels) return
        setIsUploading(true)

        const blob = await getCroppedImage(imgSrc, croppedAreaPixels)
        const formData = new FormData()
        formData.append("file", blob, "image.jpg")

        await uploadFn(formData)

        setIsUploading(false)
        setImgSrc(null)
        onClose()
    }, [croppedAreaPixels, imgSrc, onClose, setImgSrc, uploadFn])

    return (
        <ModalStyled
            isOpen={isOpen}
            onOpenChange={() => {
                if (!isUploading) {
                    onOpenChange()
                }
            }}
            isDismissable={!isUploading}
        >
            <ModalContent className="w-full min-w-fit p-4">
                <ModalHeader className="px-3 py-2 self-center">{label}</ModalHeader>
                <ModalBody>
                    {imgSrc && (
                        <div className="flex flex-col items-center gap-4">
                            <ImageCropper
                                imgSrc={imgSrc}
                                onCropComplete={setCroppedAreaPixels}
                                aspect={aspect}
                                cropShape={cropShape}
                                cropSize={cropSize}
                            />
                            <div className="flex gap-3">
                                <ButtonStyled
                                    color="primary"
                                    onPress={handleUpload}
                                    isDisabled={isUploading}
                                >
                                    {isUploading ? <Spinner /> : label}
                                </ButtonStyled>
                                <ButtonStyled onPress={onClose} isDisabled={isUploading}>
                                    {t("common.cancel")}
                                </ButtonStyled>
                            </div>
                        </div>
                    )}
                </ModalBody>
            </ModalContent>
        </ModalStyled>
    )
}
