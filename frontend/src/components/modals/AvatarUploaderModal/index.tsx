"use client"
import { AvatarCropper, ButtonStyled, ModalStyled } from "@/components/"
import { useAvatarUploadDiscloresureSingleton, useUploadAvatar } from "@/hooks"
import { getCroppedAvatar } from "@/utils/helpers/avatar"
import { ModalBody, ModalContent, ModalHeader } from "@heroui/react"
import React, { useCallback } from "react"
import { useTranslation } from "react-i18next"

type AvatarUploaderModalProps = {
    imgSrc: string | null
    setImgSrc: (imgSrc: string | null) => void
    croppedAreaPixels: any
    setCroppedAreaPixels: (area: any) => void
}

export function AvatarUploaderModal({
    imgSrc,
    setImgSrc,
    croppedAreaPixels,
    setCroppedAreaPixels
}: AvatarUploaderModalProps) {
    const { t } = useTranslation()
    const { isOpen, onOpenChange, onClose } = useAvatarUploadDiscloresureSingleton()
    const uploadImageMutation = useUploadAvatar({ onSuccess: undefined })

    // call api
    const handleUploadImage = useCallback(async () => {
        const croppedBlob = await getCroppedAvatar(imgSrc!, croppedAreaPixels)
        const formData = new FormData()
        formData.append("file", croppedBlob, "avatar.jpg")

        await uploadImageMutation.mutateAsync(formData)
        onClose()
        setImgSrc(null)
    }, [croppedAreaPixels, imgSrc, setImgSrc, onClose, uploadImageMutation])

    return (
        <ModalStyled
            isOpen={isOpen}
            onOpenChange={() => {
                if (!uploadImageMutation.isPending) {
                    onOpenChange()
                }
            }}
            isDismissable={!uploadImageMutation.isPending}
        >
            <ModalContent className="max-w-100 w-full">
                <ModalHeader className="flex flex-col gap-1">{t("user.upload_avatar")}</ModalHeader>
                <ModalBody>
                    {imgSrc && (
                        <div className="flex flex-col items-center gap-4">
                            <AvatarCropper
                                imgSrc={imgSrc}
                                setCroppedAreaPixels={setCroppedAreaPixels}
                                // setImgSrc={setImgSrc}
                            />
                            <div className="flex gap-3">
                                <ButtonStyled
                                    color="primary"
                                    onPress={handleUploadImage}
                                    isDisabled={uploadImageMutation.isPending}
                                >
                                    {t("user.upload_avatar")}
                                </ButtonStyled>
                                <ButtonStyled
                                    onPress={onClose}
                                    isDisabled={uploadImageMutation.isPending}
                                >
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
