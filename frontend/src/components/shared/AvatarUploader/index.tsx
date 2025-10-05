export * from "./AvatarCropper"
export * from "./AvatarUploadButton"

// "use client"

// import React, { useState } from "react"
// import { AvatarUploadButton } from "./AvatarUploadButton"
// import { AvatarUploaderModal } from "@/components"
// import { useAvatarUploadDiscloresureSingleton } from "@/hooks"

// export function AvatarUploader() {
//     // const { t } = useTranslation()
//     // const uploadImageMutation = useUploadAvatar({ onSuccess: undefined })

//     // const [imgSrc, setImgSrc] = useState<string | null>(null)
//     // const [croppedAreaPixels, setCroppedAreaPixels] = useState<any>(null)
//     const { onOpen } = useAvatarUploadDiscloresureSingleton()

//     const handleSelectFile = (file: File) => {
//         const reader = new FileReader()
//         reader.addEventListener("load", () => {
//             // reset when choose new one
//             setImgSrc(reader.result as string)
//             onOpen()
//             // setCrop({ x: 0, y: 0 })
//             // setZoom(1)
//         })
//         reader.readAsDataURL(file)
//     }

//     // call api
//     // const handleUploadImage = useCallback(async () => {
//     //     const croppedBlob = await getCroppedAvatar(imgSrc!, croppedAreaPixels)
//     //     const formData = new FormData()
//     //     formData.append("file", croppedBlob, "avatar.jpg")

//     //     await uploadImageMutation.mutateAsync(formData)
//     //     onClose()
//     //     setImgSrc(null)
//     // }, [croppedAreaPixels, imgSrc, onClose, uploadImageMutation])

//     return <AvatarUploadButton onFileSelect={handleSelectFile} />
// }
