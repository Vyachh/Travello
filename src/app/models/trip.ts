export interface ITrip {
    id: number
    userId: string
    title: string
    description: string
    dateFrom: string
    dateTo: string
    author: string
    image: File | null
    imageUrl: string
    isNextTrip: boolean
    isOngoingTrip: boolean
    isSelected: boolean
}