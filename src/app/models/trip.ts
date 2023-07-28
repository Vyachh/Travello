export interface ITrip {
    userId: string
    title: string
    description: string
    dateFrom: string
    dateTo: string
    author: string
    image: File | null
    isNextTrip: boolean
    isOngoingTrip: boolean
    imageUrl: string
}