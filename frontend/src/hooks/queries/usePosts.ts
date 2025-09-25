import { QUERY_KEYS } from "@/constants/queryKey"
import {
    CreatePostRequest,
    GetAllPostsResponse,
    UpdatePostRequest
} from "@/models/Post/schema/post"
import { postApi } from "@/services/postApi"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export const useGetAllPosts = () => {
    return useQuery({
        queryKey: QUERY_KEYS.POSTS,
        queryFn: async () => {
            const res = await postApi.getAll()
            return res
        }
    })
}

export const useGetPostById = ({ postId }: { postId: number }) => {
    return useQuery({
        queryKey: QUERY_KEYS.POST(postId),
        queryFn: async () => {
            const res = await postApi.getById(postId)
            return res
        }
    })
}

export const useCreatePost = ({ onSuccess }: { onSuccess?: () => void } = {}) => {
    const queryClient = useQueryClient()
    const { t } = useTranslation()

    return useMutation({
        mutationFn: (newPost: CreatePostRequest) => postApi.create(newPost),
        onSuccess: () => {
            // queryClient.setQueryData<GetAllPostsResponse>(QUERY_KEYS.POSTS, (old) => {
            //     return old ? [...old, data] : [data]
            // })
            queryClient.invalidateQueries({ queryKey: QUERY_KEYS.POSTS })
            toast.success(t("success.create"))
            onSuccess?.()
        },
        onError: (error) => {
            toast.error(error?.message || t("failed.create"))
        }
    })
}

export const useUpdatePost = ({ onSuccess }: { onSuccess?: () => void } = {}) => {
    const queryClient = useQueryClient()
    const { t } = useTranslation()

    return useMutation({
        mutationFn: (updatedPost: UpdatePostRequest) => postApi.update(updatedPost),
        onMutate: async (updatedPost) => {
            await queryClient.cancelQueries({ queryKey: QUERY_KEYS.POSTS })
            const previousPosts = queryClient.getQueryData<GetAllPostsResponse>(QUERY_KEYS.POSTS)

            queryClient.setQueryData<GetAllPostsResponse>(QUERY_KEYS.POSTS, (old) =>
                old?.map((p) => (p.id === updatedPost.id ? { ...p, ...updatedPost } : p))
            )

            toast.success(t("success.update"))
            onSuccess?.()
            return { previousPosts }
        },
        onError: (_err, _variables, context) => {
            // roll back
            if (context?.previousPosts) {
                queryClient.setQueryData<GetAllPostsResponse>(
                    QUERY_KEYS.POSTS,
                    context.previousPosts
                )
            }
            toast.error(t("failed.update"))
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: QUERY_KEYS.POSTS })
        }
    })
}

export const useDeletePost = ({ onSuccess }: { onSuccess?: () => void } = {}) => {
    const queryClient = useQueryClient()
    const { t } = useTranslation()

    return useMutation({
        mutationFn: (postId: number) => postApi.delete(postId),
        // before call API
        onMutate: async (postId: number) => {
            await queryClient.cancelQueries({ queryKey: QUERY_KEYS.POSTS })
            const previousPosts = queryClient.getQueryData<GetAllPostsResponse>(QUERY_KEYS.POSTS)

            queryClient.setQueryData<GetAllPostsResponse>(QUERY_KEYS.POSTS, (old) => {
                return old?.filter((p) => p.id !== postId) ?? []
            })

            onSuccess?.()
            toast.success(t("success.delete"))
            return { previousPosts }
        },
        onError: (_err, _postId, context) => {
            if (context?.previousPosts) {
                queryClient.setQueryData<GetAllPostsResponse>(
                    QUERY_KEYS.POSTS,
                    context.previousPosts
                ) // rollback
            }
            toast.error(t("failed.delete"))
        },
        // finally
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: QUERY_KEYS.POSTS })
        }
    })
}
