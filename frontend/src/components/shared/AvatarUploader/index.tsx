"use client"

import React from "react"
import { useImageUploadModal, useUploadAvatar } from "@/hooks"
import { ImageUploadButton } from "../ImageUploader"
import { ImageUploaderModal } from "@/components/modals"
import { useTranslation } from "react-i18next"

export function AvatarUploader() {
    const { t } = useTranslation()
    const { imgSrc, setImgSrc, isOpen, onOpenChange, onClose, onFileSelect } = useImageUploadModal({
        onBeforeOpenModal: undefined
    })
    const uploadAvatar = useUploadAvatar({ onSuccess: undefined })

    return (
        <>
            <ImageUploadButton label={t("user.upload_avatar")} onFileSelect={onFileSelect} />
            <ImageUploaderModal
                imgSrc={imgSrc}
                setImgSrc={setImgSrc}
                isOpen={isOpen}
                onOpenChange={onOpenChange}
                onClose={onClose}
                uploadFn={uploadAvatar.mutateAsync}
                aspect={1}
                cropShape="rect"
                cropSize={{ width: 700, height: 400 }}
                label={t("user.upload_avatar")}
            />
        </>
    )
}
